using AmbientZero.Data;
using AmbientZero.Data.Constants;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Transactions;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.RemoteResponse;
using AmbientZero.Service.Models.Response;
using System.Net.Http.Json;
using System.Text.Json;

namespace AmbientZero.Service.Services
{
    public class VersioningService : IVersioningService
    {
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly IApplicationVersionRepository _applicationVersionRepository;
        private readonly ITransactionScope _transactionScope;
        public VersioningService(IApplicationSettingRepository applicationSettingRepository,
            IApplicationVersionRepository applicationVersionRepository,
            ITransactionScope transactionScope)
        {
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository));
            _applicationVersionRepository = applicationVersionRepository ?? throw new ArgumentNullException(nameof(applicationVersionRepository));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
        }

        public async Task<IEnumerable<VersioningResponse>?> UpdateRemoteVersion()
        {
            using var client = new HttpClient();
            var remoteServerInformation = _applicationSettingRepository.GetApplicationSettingsCollection(ApplicationSettingsConstants.VersionUpdateSettings);
            var applicationVersion = _applicationVersionRepository.GetApplicationVersions();

            try
            {
                string url = remoteServerInformation.First(x => x.SettingName == ApplicationSettingsConstants.RootProjectURL).SettingValue!;
                string versionEndpoint = remoteServerInformation.First(x => x.SettingName == ApplicationSettingsConstants.VersionUpdateEndpoint).SettingValue!;
                string remoteDeviceId = remoteServerInformation.First(x => x.SettingName == ApplicationSettingsConstants.RemoteDeviceId).SettingValue!;
                var response = client.GetFromJsonAsync<VersionInfoRemoteResponse[]>(string.Format("{0}{1}{2}", url, versionEndpoint, remoteDeviceId), new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                _transactionScope.Execute(() =>
                {
                    foreach (var remoteApplicationVersion in response.Result)
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

            if (applicationVersion is null)
            {
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
    }
}
