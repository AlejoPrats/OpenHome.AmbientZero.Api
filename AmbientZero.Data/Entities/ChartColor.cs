using AmbientZero.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Entities
{
    public class ChartColor
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Priority { get; set; }
        public bool IsUsed { get; set; }

        internal static void Map(EntityTypeBuilder<ChartColor> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.ToTable("chartColor");
        }

        internal static void Seed(EntityTypeBuilder<ChartColor> entityBuilder)
        {
            entityBuilder.HasData(
                new ChartColor { Id = 1, Color = "#5070dd", Priority = 1, IsUsed = false },
                new ChartColor { Id = 2, Color = "#b6d634", Priority = 1, IsUsed = false },
                new ChartColor { Id = 3, Color = "#505372", Priority = 1, IsUsed = false },
                new ChartColor { Id = 4, Color = "#ff994d", Priority = 1, IsUsed = false },
                new ChartColor { Id = 5, Color = "#0ca8df", Priority = 1, IsUsed = false },
                new ChartColor { Id = 6, Color = "#ffd10a", Priority = 1, IsUsed = false },
                new ChartColor { Id = 7, Color = "#fb628b", Priority = 1, IsUsed = false },
                new ChartColor { Id = 8, Color = "#785db0", Priority = 1, IsUsed = false },
                new ChartColor { Id = 9, Color = "#3fbe95", Priority = 1, IsUsed = false },
                new ChartColor { Id = 10, Color = "#1d3997", Priority = 2, IsUsed = false },
                new ChartColor { Id = 11, Color = "#6f841a", Priority = 2, IsUsed = false },
                new ChartColor { Id = 12, Color = "#303144", Priority = 2, IsUsed = false },
                new ChartColor { Id = 13, Color = "#c75500", Priority = 2, IsUsed = false },
                new ChartColor { Id = 14, Color = "#076485", Priority = 2, IsUsed = false },
                new ChartColor { Id = 15, Color = "#9f8100", Priority = 2, IsUsed = false },
                new ChartColor { Id = 16, Color = "#cc053a", Priority = 2, IsUsed = false },
                new ChartColor { Id = 17, Color = "#46346c", Priority = 2, IsUsed = false },
                new ChartColor { Id = 18, Color = "#257259", Priority = 2, IsUsed = false },
                new ChartColor { Id = 19, Color = "#96a9ea", Priority = 3, IsUsed = false },
                new ChartColor { Id = 20, Color = "#d3e685", Priority = 3, IsUsed = false },
                new ChartColor { Id = 21, Color = "#8f92b0", Priority = 3, IsUsed = false },
                new ChartColor { Id = 22, Color = "#fec194", Priority = 3, IsUsed = false },
                new ChartColor { Id = 23, Color = "#62cff6", Priority = 3, IsUsed = false },
                new ChartColor { Id = 24, Color = "#ffe36c", Priority = 3, IsUsed = false },
                new ChartColor { Id = 25, Color = "#fca0b9", Priority = 3, IsUsed = false },
                new ChartColor { Id = 26, Color = "#ae9dcf", Priority = 3, IsUsed = false },
                new ChartColor { Id = 27, Color = "#8bd8bf", Priority = 3, IsUsed = false }
            );
        }
    }
}
