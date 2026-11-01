using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data
{
    public class DatabaseInfrastructure : IDatabaseInfrastructure
    {
        public DatabaseInfrastructure() {}

        public void Register(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AmbientZeroContext>(options => options.UseSqlite(connectionString));
        }

        public void RunMigrations(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AmbientZeroContext>();
            context.Database.Migrate();
            context.Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(FULL);");
            context.Database.CloseConnection();
        }
    }
}
