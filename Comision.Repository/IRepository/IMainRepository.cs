using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Comision.Repository.IRepository
{
    public interface IMainRepository<ObjectDomainClass> : IDisposable where ObjectDomainClass : class
    {
        IQueryable<ObjectDomainClass> All();
        IEnumerable<ObjectDomainClass> All(Func<ObjectDomainClass, ObjectDomainClass> selector = null);
        IQueryable<ObjectDomainClass> Where(Expression<Func<ObjectDomainClass, bool>> predicate);
        IQueryable<ObjectDomainClass> PagedResult(IQueryable<ObjectDomainClass> query, 
                        Expression<Func<ObjectDomainClass, bool>> orderByProperty, bool isAscendingOrder, out int rowsCount, int pageNum = 1, int pageSize = 20);
        IEnumerable<ObjectDomainClass> Where(Expression<Func<ObjectDomainClass, bool>> predicate, Func<ObjectDomainClass, ObjectDomainClass> selector);
        IQueryable<ObjectDomainClass> Include(string path);
        bool Contains(Expression<Func<ObjectDomainClass, bool>> predicate);
        ObjectDomainClass Find(Expression<Func<ObjectDomainClass, bool>> predicate);
        ObjectDomainClass Find(Expression<Func<ObjectDomainClass, bool>> predicate, Func<ObjectDomainClass, ObjectDomainClass> selector);
        ObjectDomainClass Add(ObjectDomainClass objectClassTable);
        IQueryable<ObjectDomainClass> Include<TProperty>(Expression<Func<ObjectDomainClass, TProperty>> expression);
        void Add(List<ObjectDomainClass> objectClassTable);
        int Delete(ObjectDomainClass objectClassTable);
        int Delete(Expression<Func<ObjectDomainClass, bool>> predicate);
      //  int Delete(IQueryable<ObjectDomainClass> query);
        int Update(ObjectDomainClass objectClassTable);
      //  int Update(Expression<Func<ObjectDomainClass, bool>> filterExpression, Expression<Func<ObjectDomainClass, ObjectDomainClass>> updateExpression);
      //  int Update(IQueryable<ObjectDomainClass> query, Expression<Func<ObjectDomainClass, ObjectDomainClass>> updateExpression);
        
        int AddOrUpdate(params ObjectDomainClass[] entities);
        int AddOrUpdate(Expression<Func<ObjectDomainClass, object>> identifierExpression, params ObjectDomainClass[] entities);
        int Count { get; }
        void ChangeEntityStates(EntityState state, ObjectDomainClass objectClassTable);
        void ChangeEntityStates(EntityState state, List<ObjectDomainClass> objectClassTables);
    }
}
