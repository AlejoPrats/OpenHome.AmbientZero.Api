using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        public DateRangeTimezone GetSartTimezone(DateTime date, string timeZoneId)
        {
            var localMidnight = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Unspecified);
            var midnightInZone =
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(localMidnight, timeZoneId, TimeZoneInfo.Utc.Id);
            return new DateRangeTimezone() { DayStart = midnightInZone };
        }

        public DateTime GetTimeFromTimezone(DateTime date, string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
            {
                return date;
            }

            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Utc.Id, timeZoneId);
        }
    }
}