using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Entities
{
    public class MigrationReference
    {
        public int Id { get; set; }
        public string OriginalId { get; set; }
        public string NewId { get; set; }

        internal static void Map(EntityTypeBuilder<MigrationReference> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.ToTable("migrationReferenceId");
        }
    }
}
