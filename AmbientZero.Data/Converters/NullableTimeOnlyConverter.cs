using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Converters
{
    /// <summary>
    /// <para>
    /// EF Core (especially with SQLite) does not correctly map nullable time columns to TimeOnly?.
    /// When the database contains NULL, EF may materialize the value as TimeOnly.MinValue
    /// instead of null, because empty strings or invalid values are parsed as the default struct.
    /// This converter ensures that:
    /// </para>
    /// <list type="bullet">
    /// <item><description>NULL in the database becomes null in C#</description></item>
    /// <item><description>null in C# is written as NULL in the database</description></item>
    /// <item><description>Valid times round-trip using the "HH:mm:ss" format</description></item>
    /// </list>
    /// <para>
    /// This prevents accidental MinValue values from appearing where nulls are expected.
    /// </para>
    /// </summary>
    public class NullableTimeOnlyConverter : ValueConverter<TimeOnly?, string?>
    {
        public NullableTimeOnlyConverter() : base(
            v => v.HasValue ? v.Value.ToString("HH:mm:ss") : null,
            v => string.IsNullOrWhiteSpace(v) ? null : TimeOnly.Parse(v))
        {
        }
    }
}
