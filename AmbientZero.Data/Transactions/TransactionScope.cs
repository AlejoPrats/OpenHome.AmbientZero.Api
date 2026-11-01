using AmbientZero.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Data.Transactions
{
    public class TransactionScope : ITransactionScope
    {
        private readonly AmbientZeroContext _ambientZeroContext;

        public TransactionScope(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext));
        }

        public void Execute(Action action) 
        {
            using var transaction = _ambientZeroContext.Database.BeginTransaction();

            try 
            {
                action();
                _ambientZeroContext.SaveChanges();
                transaction.Commit();
            }
            catch 
            { 
                transaction.Rollback();
                throw;
            }
        }
    }
}
