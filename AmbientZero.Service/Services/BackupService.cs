using AmbientZero.Data.Backup;
using AmbientZero.Data.Backup.ScriptGenerators;
using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using AmbientZero.Data.Transactions;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models;
using AmbientZero.Service.Models.Response;
using AmbientZero.Service.Models.Response.Export;

namespace AmbientZero.Service.Services
{
    public class BackupService : IBackupService
    {
        private readonly IBackupRepository _backupRepository;
        private readonly IApplicationSettingRepository _applicationSettingRepository;
        private readonly IBashScriptsService _bashScriptsService;
        private readonly ITransactionScope _transactionScope;
        private readonly ITemporalJsonService<ProcessProgress> _temporalJsonService;
        private const int _pageSize = 2000;

        public BackupService(IBackupRepository backupRepository,
            IApplicationSettingRepository applicationSettingRepository,
            IBashScriptsService bashScriptsService,
            ITransactionScope transactionScope,
            ITemporalJsonService<ProcessProgress> temporalJsonService)
        {
            _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
            _applicationSettingRepository = applicationSettingRepository ?? throw new ArgumentNullException(nameof(applicationSettingRepository));
            _bashScriptsService = bashScriptsService ?? throw new ArgumentNullException(nameof(bashScriptsService));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
            _temporalJsonService = temporalJsonService ?? throw new ArgumentNullException(nameof(temporalJsonService));
        }

        public void GenerateBackup()
        {
            _temporalJsonService.UpdateState(new ProcessProgress() { Percentage = 0, Status = "Starting" });
            _temporalJsonService.SaveBackground();
            _bashScriptsService.OfflineExecution(ExecutionModeConstants.Export);
        }

        public bool GenerateBackupFile(string[]? deviceIds = null)
        {
            IEnumerable<DeviceInformation>? devices;

            if (deviceIds == null)
            {
                devices = _backupRepository.GetDevicesInformation();
            }
            else
            {
                devices = _backupRepository.GetDeviceInformationById(deviceIds);
            }

            if (devices is null)
            {
                return false;
            }

            var devicesIds = devices.Select(x => x.DeviceId).ToArray();

            var backupData = new BackupModel();
            var idSwitchboard = new IdSwitchboard(devices.Select(x => x.DeviceId).ToArray());
            var sensorQueryGenerator = new SensorQueryGenerator(devices.Count());
            var sensorSettingsQueryGenerator = new SensorSettingsQueryGenerator(devices.Count());
            var measurementLogQueryGenerator = new MeasurementLogQueryGenerator();
            var migrationReferenceQueryGenerator = new MigrationReferenceQueryGenerator(devices.Count());
            var measurementsCount = _backupRepository.GetMeasurementsCount(devicesIds);
            var backupFile = new BackupFileWriter(BackupSettings.GeneratedBackupPath, BackupSettings.GeneratedBackupFileName);

            foreach (var sensor in devices)
            {
                var deviceId = idSwitchboard.Switcharoo(sensor.DeviceId);
                sensorQueryGenerator.Append(deviceId,
                    sensor.DeviceVirtualName,
                    sensor.Version ?? string.Empty);
                sensorSettingsQueryGenerator.Append(deviceId,
                    sensor.Settings.DisableEndTime,
                    sensor.Settings.DisableStartTime,
                    sensor.Settings.Color,
                    sensor.Settings.IsLightEnabled,
                    sensor.Settings.IsLightOnSchedule);
            }

            _temporalJsonService.UpdateState(new ProcessProgress() { Percentage = 0, Status = "Processing Backup" });

            var pages = (double)measurementsCount / _pageSize;
            backupFile.WriteMetadata(DateTime.UtcNow, devices.Count(), measurementsCount, (int)Math.Ceiling(pages));
            backupFile.WriteSection(sensorSettingsQueryGenerator.FinishSqlScript());
            backupFile.WriteSection(sensorQueryGenerator.FinishSqlScript());

            for (int i = 0; (_pageSize * i) < measurementsCount; i++)
            {
                var measurements = _backupRepository.GetMeasurements(devicesIds, (_pageSize * i), _pageSize);
                measurementLogQueryGenerator.CreateHeader(measurements!.Count(), i);
                foreach (var measurement in measurements)
                {
                    measurementLogQueryGenerator.Append(idSwitchboard.Switcharoo(measurement.DeviceId), measurement.Temperature, measurement.Date);
                }
                backupFile.WriteSection(measurementLogQueryGenerator.FinishSqlScript());
            }

            backupData.MigrationIds = idSwitchboard.GetMap();

            foreach (var keyValuePair in idSwitchboard.GetMap())
            {
                migrationReferenceQueryGenerator.Append(keyValuePair.Key, keyValuePair.Value);
            }

            backupFile.WriteSection(migrationReferenceQueryGenerator.FinishSqlScript());

            backupFile.Dispose();

            _applicationSettingRepository.UpdateApplicationSetting(ApplicationSettingsConstants.LastBackupDate, DateTime.UtcNow.ToString());

            return true;
        }

        public BackupFileResponse? GetFileStream()
        {
            var filePath = Path.Combine(BackupSettings.GeneratedBackupPath, BackupSettings.GeneratedBackupFileName);
            var result = new BackupFileResponse();
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.CopyTo(result.BackupStream);
                }

                string lastBackupDate = _applicationSettingRepository.GetApplicationSettingByKey(ApplicationSettingsConstants.LastBackupDate)?.SettingValue ?? "unknown";
                result.Filename = $"{BackupSettings.GeneratedBackupFileName}_{lastBackupDate}";

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void ImportBackup()
        {
            _bashScriptsService.OfflineExecution(ExecutionModeConstants.Import);
        }

        public bool BackupFileUpload(Stream file)
        {
            try
            {
                if (!Directory.Exists(BackupSettings.UploadBackupPath))
                {
                    Directory.CreateDirectory(BackupSettings.UploadBackupPath);
                }

                string filePath = Path.Combine(BackupSettings.UploadBackupPath, BackupSettings.ImportedBackupFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public BackupImportResponse ImportBackupFile()
        {
            var result = new BackupImportResponse() { IsSuccess = true };
            string filePath = Path.Combine(BackupSettings.UploadBackupPath, BackupSettings.ImportedBackupFileName);

            var backupFileProcessing = BackupFileProcessing
                .SetFileLocation(filePath)
                .Open();

            try
            {
                _transactionScope.Execute(() =>
                {
                    while (backupFileProcessing.HasMoreBlocks)
                    {
                        var sql = backupFileProcessing.ReadNextBlock();
                        _backupRepository.ExecuteSqlRaw(sql!);
                    }
                });
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
            finally
            {
                backupFileProcessing.CloseAndDelete();
            }

            return result;
        }
    }
}
