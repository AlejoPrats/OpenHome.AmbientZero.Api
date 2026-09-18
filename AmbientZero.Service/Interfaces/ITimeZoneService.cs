using AmbientZero.Service.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface ITimeZoneService
    {
        DateRangeTimezone GetSartTimezone(DateTime date, string timeZoneId);
        DateTime GetTimeFromTimezone(DateTime date, string timeZoneId);
    }
}
