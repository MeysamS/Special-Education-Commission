using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Comision.Data.Context;
using Comision.Repository.IRepository;
using EntityFramework.Extensions;

namespace Comision.Repository.ImplementationRepository
{
    public class MainRepository<TEntity> : IMainRepository<TEntity> where TEntity : class
    {
        private bool _shareContext = false;
        protected IUnitOfWork Context;

        protected readonly IDbSet<TEntity> _dbset;

        //public MainService()
        //{
        //    Context = new DBContext();
        //    shareContext = false;
        //}
        public MainRepository(IUnitOfWork context)
        {
            Context = context;
            _shareContext = true;
        }
        protected DbSet<TEntity> DbSet
        {
            get
            {
                return ((DbSet<TEntity>)Context.Set<TEntity>());
            }
        }
        
        /// <summary>
        /// بازیابی تمام رکورد ها
        /// </summary>
        /// <returns> کویری </returns>
        public virtual IQueryable<TEntity> All()
        {
            return DbSet.AsQueryable();
        }

        /// <summary>
        /// بازیابی فیلد های خاص همه رکورد ها
        /// </summary>
        /// <param name="selector">فیلد هایی که در خروجی نیاز دارید</param>
        /// <returns>لیست</returns>
        public virtual IEnumerable<TEntity> All(Func<TEntity, TEntity> selector = null)
        {
            
            return DbSet.Select(selector);
        }
        /// <summary>
        /// بازیابی رکورد های خاص بر اساس شرط
        /// </summary>
        /// <param name="predicate">شرط لینک یو</param>
        /// <returns>کویری</returns>
        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
           
            return DbSet.Where(predicate);
        }
       

         /// <summary>
         /// نمایش اطلاعات یک کویری به صورت صفحه بندی
         /// در مواقعی که قصد نمایش اطلاعات را داریم فقط این روش خوب است و سرع دارد چون کش را غیر غعال می کند
         /// </summary>
         /// <param name="query">کویری و شرزط های مورد نظر</param>
         /// <param name="pageNum">شماره صفحه ای که می خواهید</param>
         /// <param name="pageSize">اندازه صفحه بندی چند تا چند تا صفحه بندی شود</param>
         /// <param name="orderByProperty">فیلد  هایمورد نظر برای مرتب سازی</param>
         /// <param name="isAscendingOrder">ایا مرتب سازی نزولی انجام شود؟</param>
        /// <param name="rowsCount">تعداد کل صفحه پیدا شده</param>
         /// <returns></returns>
        public virtual IQueryable<TEntity> PagedResult(IQueryable<TEntity> query, 
                       Expression<Func<TEntity, bool>> orderByProperty, bool isAscendingOrder, out int PageCount,int pageNum=1, int pageSize=20)
        {
            if (pageSize <= 0) pageSize = 20;

            //مجموع صفحه های به دست آمده
            int rowCount = query.Count();
            if (rowCount < pageSize)
                PageCount = 1;
            else
            {
                int pagecount = 0;
                pagecount = rowCount / pageSize;
                int mod = rowCount % pageSize > 0 ? 1 : 0;
                PageCount = pagecount + mod;
            }

           
            // اگر شماره صفحه کوچکتر از 0 بود صفحه اول نشان داده شود
            if (rowCount <= pageSize || pageNum <= 0) pageNum = 1;

            // محاسبه ردیف هایی که نسبت به سایز صفحه باید از آنها گذشت
            int excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            // ردشدن از ردیف‌های اضافی و  دریافت ردیف‌های مورد نظر برای صفحه مربوطه
            return query.Skip(excludedRows).Take(pageSize);
        }

        /// <summary>
        /// بازیابی رکورد ها بر اساس شرط و بازیابی برخی فیلد ها
        /// </summary>
        /// <param name="predicate">شرط</param>
        /// <param name="selector">فیلد های مورد نیاز</param>
        /// <returns>خروجی لیست </returns>
        public virtual IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, Func<TEntity, TEntity> selector)
        {

            return DbSet.Where(predicate).Select(selector);
        }

        /// <summary>
        /// بررسی وجود یک رکورد خاص در جدول
        /// </summary>
        /// <param name="predicate">شرط</param>
        /// <returns>اگر وجود دارد مقدار صحیح را ارسال می کند</returns>
        public virtual bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            
            return DbSet.Count(predicate) > 0;
        }

        /// <summary>
        /// جستجوی یک رکورد بر اساس شرط خاص
        /// </summary>
        /// <param name="predicate">شرط</param>
        /// <returns>رکورد خاص یا شی مورد نطر را ارسال می کند اگر وجود دارد </returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        ///بازیابی یک رکورد خاص براساس شرط و اینکه فقط برخی از فیلد های آن بازیابی شود
        /// </summary>
        /// <param name="predicate">شرط</param>
        /// <param name="selector">پراپرتی یا فیلد هایی که نیاز داریم</param>
        /// <returns>کلاس یا شی مورد نظر به همراه فیلد های درخواستی بقیه فیلد ها مقدار نال دارند</returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate, Func<TEntity, TEntity> selector)
        {
            return DbSet.Where(predicate).Select(selector).Single();
        }

        public virtual IQueryable<TEntity> Include(string path)
        {
            return DbSet.Include(path);
        }
    
        /// <summary>
        /// درج یک شی یا رکورد خاص
        /// </summary>
        /// <param name="objectDepartment">کلاسی که باید درج شود</param>
        /// <returns>وفتی ذخیره شد کلاس برگشت می دهد تا مواقعی که به کلید آن نیاز داریم </returns>
        public virtual TEntity Add(TEntity objectDepartment)
        {
            return DbSet.Add(objectDepartment);
          
        }

        /// <summary>
        /// درج لیستی از رکورد ها به صورت یک جا
        /// </summary>
        /// <param name="objectDepartment">لیست رکورد ها</param>
        public virtual void Add(List<TEntity> objectTEntityService)
        {
            //اول ردیابی تعقییرات را غیر فعال میکنم تا سرعت درج بهتر شود
            //Context.AutoDetectChangesEnabled = false;
            //Context.TTBulkInsert(objectTEntityService);
            foreach (var item in objectTEntityService)
                DbSet.Add(item);
            //ردیابی را به حالت اولیه بر می گردانیم
            //Context.AutoDetectChangesEnabled = true;
        }

        /// <summary>
        /// حذف یک رکورد براساس پارامتر ورودی از نوع شی
        /// </summary>
        /// <param name="objectDepartment">شی یا رکوردی که باید حذف شود</param>
        /// <returns>تعداد رکورد های حذف شده</returns>
        public virtual int Delete(TEntity objectDepartment)
        {
            DbSet.Remove(objectDepartment);
          
            return 0;// بعدا اطلاح شود
        }

        /// <summary>
        /// حذف رکورد ها بر اساس شرط
        /// </summary>
        /// <param name="predicate">شرط</param>
        /// <returns></returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var data = DbSet.Where(predicate);
            foreach (var item in data)
                DbSet.Remove(item);

         ////ChangeEntityStates(EntityState.Deleted, data.ToList());
            
         ////   DbSet.Delete(predicate);غیر فعال کرده ایم به علت مشکل تراکنشی در اعمام Savechange()
            return 0;
        }
        /// <summary>
        /// حذف بر اساس کویری
        /// </summary>
        /// <param name="query">کویری</param>
        /// <returns></returns>
        public virtual int Delete(IQueryable<TEntity> query)
        {
            DbSet.Delete(query);
            return 0;
        }
        /// <summary>
        /// آپدیت بر اساس شی 
        /// </summary>
        /// <param name="objectDepartment">شی مورد نظر که باید ویرایش شود</param>
        /// <returns></returns>
        public virtual int Update(TEntity objectDepartment)
        {
           //var entry = Context.Entry(objectDepartment);
          DbSet.Attach(objectDepartment);
          Context.MarkAsChanged(objectDepartment);
          //////Context.Entry(objectDepartment).State = EntityState.Modified;
            return 0;
        }
        

        /// <summary>
        /// این متد فعلا به علت مشکل تراکنشی خوب عمل نمیکند
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            DbSet.Update(filterExpression, updateExpression);
            //if (!shareContext)
            //    return Context.SaveChanges();
            return 0;
        }
        /// <summary>
        /// آپدیت بر اساس کویری
        /// </summary>
        /// <param name="query">کویری</param>
        /// <param name="updateExpression">مقادیری که باید ویرایش شود</param>
        /// <returns></returns>
        public virtual int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            DbSet.Update(query, updateExpression);
            //if (!shareContext)
            //    return Context.SaveChanges();
            return 0;
        }

        public virtual int AddOrUpdate(params TEntity[] entities)
        {
            DbSet.AddOrUpdate(entities);
            //if (!shareContext)
            //    return Context.SaveChanges();
            return 0;
        }
        public virtual int AddOrUpdate(Expression<Func<TEntity, object>> identifierExpression, params TEntity[] entities)
        {
            DbSet.AddOrUpdate(identifierExpression, entities);
            //if (!shareContext)
            //    return Context.SaveChanges();
            return 0;
        }


        /// <summary>
        /// تعداد رکود های یک کلاس یا جدول
        /// </summary>
        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }
       
        public void Dispose()
        {
            if (DbSet != null)
                Context.Dispose();
        }


        public virtual IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return DbSet.Include(expression);
            throw new NotImplementedException();
        }
        public void ChangeEntityStates(EntityState state, TEntity objectDepartment)
        {
             Context.ChangeEntityStates(state,objectDepartment);
        }
        public void ChangeEntityStates(EntityState state, List<TEntity> objectDepartment)
        {
            foreach (TEntity t in objectDepartment)
                Context.ChangeEntityStates(state, t);
        }
    }
}
