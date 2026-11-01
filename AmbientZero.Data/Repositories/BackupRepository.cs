using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Repositories
{
    public class BackupRepository : IBackupRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;

        public BackupRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext));
        }

        public IEnumerable<DeviceInformation>? GetDevicesInformation()
        { 
            return _ambientZeroContext
                .Devices
                .Include(s => s.Settings)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<DeviceInformation>? GetDeviceInformationById(string[] deviceIds)
        {
            return _ambientZeroContext
                .Devices
                .Include(s => s.Settings)
                .AsNoTracking()
                .Where(x => deviceIds.Contains(x.DeviceId))
                .ToList();
        }

        public IEnumerable<MeasurementLog>? GetMeasurements(string[] deviceIds, int skip, int take)
        {
            return _ambientZeroContext.Measurements
                .AsNoTracking()
                .Where(x => deviceIds.Contains(x.DeviceId))
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public int GetMeasurementsCount(string[] deviceIds)
        {
            return _ambientZeroContext.Measurements
                .AsNoTracking()
                .Where(x => deviceIds.Contains(x.DeviceId)).Count();
        }

        public void ExecuteSqlRaw(string sql)
        {
            _ambientZeroContext.Database.ExecuteSqlRaw(sql);
        }
    }
}
