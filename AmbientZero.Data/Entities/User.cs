using AmbientZero.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AmbientZero.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }

        internal static void Map(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityBuilder.ToTable("users");
        }

        internal static void Seed(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasData(
                new User { Id = 1, Username = "Admin", Password = "", IsActive = false, CreatedOn = new DateTime(1970, 1, 1) }
                );
        }
    }
}
