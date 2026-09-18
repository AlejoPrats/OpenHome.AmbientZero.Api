using AmbientZero.Data.Backup;
using AmbientZero.Data.Backup.ScriptGenerators;
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
        private readonly ITransactionScope _transactionScope;
        private const int _pageSize = 2000;

        public BackupService(IBackupRepository backupRepository, ITransactionScope transactionScope)
        {
            _backupRepository = backupRepository ?? throw new ArgumentNullException(nameof(backupRepository));
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
        }

        public BackupFileResponse? GenerateBackupFile(string[] deviceIds)
        {
            var devices = _backupRepository.GetDeviceInformationById(deviceIds);

            if (devices is null)
            {
                return null;
            }

            var devicesIds = devices.Select(x => x.DeviceId).ToArray();

            var backupData = new BackupModel();
            var idSwitchboard = new IdSwitchboard(devices.Select(x => x.DeviceId).ToArray());
            var sensorQueryGenerator = new SensorQueryGenerator(devicesIds.Length);
            var sensorSettingsQueryGenerator = new SensorSettingsQueryGenerator(devices.Count());
            var measurementLogQueryGenerator = new MeasurementLogQueryGenerator();
            var migrationReferenceQueryGenerator = new MigrationReferenceQueryGenerator(deviceIds.Length);
            var measurementsCount = _backupRepository.GetMeasurementsCount(devicesIds);
            var backupFile = new BackupFileWriter(BackupSettings.GeneratedBackupPath, BackupSettings.FileName);

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

            var pages = (double)measurementsCount / _pageSize;
            backupFile.WriteMetadata(DateTime.UtcNow, devicesIds.Length, measurementsCount, (int)Math.Ceiling(pages));
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

            return new BackupFileResponse()
            {
                Filename = BackupSettings.FileName,
                BackupStream = backupFile.BackupStream
            };
        }

        public BackupImportResponse ImportBackup(string name, Stream file)
        {
            var result = new BackupImportResponse() { IsSuccess = true };
            var fileExtension = Path.GetExtension(name);

            if (fileExtension != string.Empty &&
                fileExtension is not BackupSettings.FileExtension)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "Filename not in the correct format";
            }

            var backupFileProcessing = BackupFileProcessing
                .Create(BackupSettings.UploadBackupPath, name)
                .Save(file)
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
