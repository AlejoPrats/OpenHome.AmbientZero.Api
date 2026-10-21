using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface ITemporalJsonService<T>
    {
        void UpdateState(T newState);
        void SaveBackground();
    }
}
