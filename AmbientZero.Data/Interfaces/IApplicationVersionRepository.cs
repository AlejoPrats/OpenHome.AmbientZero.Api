using AmbientZero.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Interfaces
{
    public interface IApplicationVersionRepository
    {
        IEnumerable<ApplicationVersion> GetApplicationVersions();
        void UpdateApplicationVersion(ApplicationVersion version);
    }
}
