using AmbientZero.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Interfaces
{
    public interface IBackupRepository
    {
        IEnumerable<DeviceInformation>? GetDevicesInformation();
        IEnumerable<DeviceInformation>? GetDeviceInformationById(string[] deviceIds);
        IEnumerable<MeasurementLog>? GetMeasurements(string[] deviceIds, int skip, int take);
        int GetMeasurementsCount(string[] deviceIds);
        void ExecuteSqlRaw(string sql);
    }
}
