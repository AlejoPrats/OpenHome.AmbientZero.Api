using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Transactions
{
    public interface ITransactionScope
    {
        void Execute(Action action);
    }
}
