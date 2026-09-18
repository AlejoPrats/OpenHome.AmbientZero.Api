using AmbientZero.Data;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Modules
{
    public class StartupModule : IStartupModule
    {
        private readonly IDatabaseInfrastructure _databaseInfrastructure;
        private readonly Secrets _secrets;
        public StartupModule(IDatabaseInfrastructure databaseInfrastructure,
            IOptions<Secrets> secrets)
        {
            _databaseInfrastructure = databaseInfrastructure ?? throw new ArgumentNullException(nameof(databaseInfrastructure));
            _secrets = secrets?.Value ?? throw new ArgumentNullException(nameof(secrets));
        }

        public void AddDbContext(IServiceCollection services)
        {
            _databaseInfrastructure.Register(services, _secrets.ConnectionString);
        }

        public void ValidateMigrations(IServiceProvider provider)
        {
            _databaseInfrastructure.RunMigrations(provider);
        }
    }
}
