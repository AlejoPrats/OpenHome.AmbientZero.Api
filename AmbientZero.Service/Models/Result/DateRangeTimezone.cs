using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Result
{
    public class DateRangeTimezone
    {
        public DateTime DayStart { get; set; }
        public DateTime DayEnd => DayStart.AddDays(1).AddTicks(-1);
    }
}
