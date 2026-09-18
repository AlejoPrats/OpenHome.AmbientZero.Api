using AmbientZero.Data;
using AmbientZero.Data.Constants;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Transactions;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.RemoteResponse;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace AmbientZero.Service.Services
{
    public class VersioningService : IVersioningService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly IApplicationVersionRepository _applicationVersionRepository;
        private readonly IBashScriptsService _bashScriptsService;
        private readonly ITransactionScope _transactionScope;
        private readonly ILogger<VersioningService> _logger;

        public VersioningService(IApplicationSettingRepository applicationSettingRepository,
            IApplicationVersionRepository applicationVersionRepository,
            ITransactionScope transactionScope,
            IBashScriptsService bashScriptsService,
            ILogger<VersioningService> logger)
        {
            _applicationSettingRepository = applicationSettingRepository ??
                                            throw new ArgumentNullException(nameof(applicationSettingRepository));
            _applicationVersionRepository = applicationVersionRepository ??
                                            throw new ArgumentNullException(nameof(applicationVersionRepository));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
            _bashScriptsService = bashScriptsService ?? throw new ArgumentNullException(nameof(bashScriptsService));
            _logger = logger;
        }

        public async Task<IEnumerable<VersioningResponse>?> UpdateRemoteVersion()
        {
            using var client = new HttpClient();
            var remoteServerInformation =
                _applicationSettingRepository.GetApplicationSettingsCollection(ApplicationSettingsConstants
                    .VersionUpdateSettings);
            var applicationVersion = _applicationVersionRepository.GetApplicationVersions();

            try
            {
                string url = remoteServerInformation!
                    .First(x => x.SettingName == ApplicationSettingsConstants.RootProjectURL).SettingValue!;
                string versionEndpoint = remoteServerInformation!
                    .First(x => x.SettingName == ApplicationSettingsConstants.VersionUpdateEndpoint).SettingValue!;
                string remoteDeviceId = remoteServerInformation!
                    .First(x => x.SettingName == ApplicationSettingsConstants.RemoteDeviceId).SettingValue!;
                var response = await client.GetFromJsonAsync<VersionInfoRemoteResponse[]>(
                    string.Format("{0}/{1}?id={2}", url, versionEndpoint, remoteDeviceId), new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                _transactionScope.Execute(() =>
                {
                    foreach (var remoteApplicationVersion in response)
                    {
                        var localVersion = applicationVersion.First(x => x.ModuleName == remoteApplicationVersion.Name);
                        Version vRemote;

                        if (Version.TryParse(localVersion.RemoteVersion, out var vLocal))
                        {
                            vRemote = Version.Parse(remoteApplicationVersion.Version);

                            if (vLocal >= vRemote)
                            {
                                continue;
                            }
                        }

                        localVersion.RemoteVersion = remoteApplicationVersion.Version;
                        localVersion.LastUpdatedOn = DateTime.UtcNow;

                        _applicationVersionRepository.UpdateApplicationVersion(localVersion);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Updating Application Version: {Message}", ex.Message);
                return null;
            }

            return applicationVersion.Select(x => new VersioningResponse()
            {
                Name = x.ModuleName,
                LastUpdated = x.LastUpdatedOn,
                LocalVersion = x.LocalVersion,
                RemoteVersion = x.RemoteVersion
            });
        }

        public IEnumerable<VersioningResponse>? GetVersions()
        {
            var applicationVersion = _applicationVersionRepository.GetApplicationVersions();

            if (applicationVersion.Any())
            {
                return applicationVersion.Select(x => new VersioningResponse()
                {
                    Id = x.ModuleId,
                    Name = x.ModuleName,
                    LastUpdated = x.LastUpdatedOn,
                    LocalVersion = x.LocalVersion,
                    RemoteVersion = x.RemoteVersion
                });
            }
            else
            {
                return null;
            }
        }
        
        public async Task UpdateModule(UpdateModuleRequest request)
        {
            switch (request.ModuleId.ToString())
            {
                case ApplicationVersionConstants.APIModuleId:
                    await _bashScriptsService.UpdateModule(ApplicationVersionConstants.APIScript,
                        "http://192.168.50.170/appRepo/ambientzeroapi.zip",request.HasFile);
                    break;
                case ApplicationVersionConstants.WebModuleId:
                    await _bashScriptsService.UpdateModule(ApplicationVersionConstants.WebScript,
                        "http://192.168.50.170/appRepo/ambientzeroweb.zip",request.HasFile);
                    break;
            }
        }

        public async Task<bool> UploadModuleFile(IFormFile file, string moduleId)
        {
            if (Path.GetExtension(file.FileName).ToLower().Trim() == ".zip")
            {
                string filePath;
                switch (moduleId)
                {
                    case ApplicationVersionConstants.APIModuleId:
                        filePath = Path.Combine("/tmp/", ApplicationVersionConstants.APIFilename);
                        break;
                    case ApplicationVersionConstants.WebModuleId:
                        filePath = Path.Combine("/tmp/", ApplicationVersionConstants.WebFilename);
                        break;
                    default:
                        return false;
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                   await file.CopyToAsync(stream);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}