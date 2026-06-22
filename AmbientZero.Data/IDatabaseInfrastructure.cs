using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data
{
    public interface IDatabaseInfrastructure
    {
        void Register(IServiceCollection services, string connectionString);
        void RunMigrations(IServiceProvider provider);
    }
}
