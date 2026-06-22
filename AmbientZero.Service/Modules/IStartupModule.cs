using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Modules
{
    public interface IStartupModule
    {
        void AddDbContext(IServiceCollection services);
        void AddAuthentication(IServiceCollection services);
        void SetResponseCompression(IServiceCollection services);
        void ValidateMigrations(IServiceProvider services);
    }
}
