using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Data.Context
{
    public abstract class DbContextBase:DbContext
    {
                  #region Fields

        private ITransaction _currenTransaction;

        #endregion

        #region NestedTypes

        private class DbContextTransactionAdapter : ITransaction
        {
            private DbContextTransaction _transaction;

            public DbContextTransactionAdapter(DbContextTransaction transaction)
            {
                Guard.NotNull(transaction, nameof(transaction));

                _transaction = transaction;
            }

            public void Commit()
            {
                _transaction?.Commit();
            }

            public void Rollback()
            {
                if (_transaction?.UnderlyingTransaction.Connection != null)
                    _transaction.Rollback();
            }

            public void Dispose()
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        #endregion

        #region Public Methods

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_currenTransaction != null) return _currenTransaction;

            return _currenTransaction = new DbContextTransactionAdapter(Database.BeginTransaction(isolationLevel));
        }
        #endregion

        #region Properties

        public ITransaction Transaction => _currenTransaction;
        public IDbConnection Connection => Database.Connection;
        public bool HasTransaction => _currenTransaction != null;

        #endregion
    }
}
