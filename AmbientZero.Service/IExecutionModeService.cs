using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service
{
    public interface IExecutionModeService
    {
        void GenerateBackup(IServiceProvider services);
        void ImportBackup(IServiceProvider services);
    }
}
