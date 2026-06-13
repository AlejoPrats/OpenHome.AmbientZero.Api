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
    public class ApplicationVersion
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string LocalVersion { get; set; }
        public string? RemoteVersion { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        internal static void Map(EntityTypeBuilder<ApplicationVersion> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.ToTable("version");
        }

        internal static void Seed(EntityTypeBuilder<ApplicationVersion> entityBuilder)
        {
            entityBuilder.HasData(
                new ApplicationVersion { Id = 1, ModuleName = ApplicationVersionConstants.API, LocalVersion = "0.0.0" },
                new ApplicationVersion { Id = 2, ModuleName = ApplicationVersionConstants.PCB, LocalVersion = "0.0.0" },
                new ApplicationVersion { Id = 3, ModuleName = ApplicationVersionConstants.Web, LocalVersion = "0.0.0" },
                new ApplicationVersion { Id = 4, ModuleName = ApplicationVersionConstants.SensorCode, LocalVersion = "0.0.0" }
                );
        }
    }


}
