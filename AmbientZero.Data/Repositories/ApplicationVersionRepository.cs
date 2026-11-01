using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Repositories
{
    public class ApplicationVersionRepository : IApplicationVersionRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;
        public ApplicationVersionRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext)); 
        }

        public IEnumerable<ApplicationVersion> GetApplicationVersions()
        {
            return _ambientZeroContext.ApplicationVersions.ToList();
        }

        public void UpdateApplicationVersion(ApplicationVersion version)
        {
            _ambientZeroContext
                .ApplicationVersions
                .Where(x => x.ModuleName == version.ModuleName)
                .ExecuteUpdate(x => 
                x.SetProperty(av => av.RemoteVersion, version.RemoteVersion)
                .SetProperty(av => av.LastUpdatedOn, version.LastUpdatedOn));
        }
    }
}
