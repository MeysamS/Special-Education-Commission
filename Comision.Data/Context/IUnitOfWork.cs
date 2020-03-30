using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Transactions;

namespace Comision.Data.Context
{
    public interface IUnitOfWork : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class;
        IList<T> GetRows<T>(string sql, params object[] parameters) where T : class;
        void ExecCommand(string query);
        IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void ForceDatabaseInitialize();
        void BeginTransaction();
        void CommitTransaction();
        void Rollback();

        void ChangeEntityStates(EntityState state,object objectDepartment);
    }
}
