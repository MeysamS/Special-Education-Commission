using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Threading;
using Comision.Model.Common;
using Comision.Model.Configuration;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Comision.Data.Context
{
    public class ComisionDbContext : IdentityDbContext<User, Role, long, UserLogin, UserRole, UserClaim>, IUnitOfWork
    {
        #region Fields

        private readonly IList<Assembly> _configurationsAssemblies = new List<Assembly>();
        private readonly IList<Type[]> _dynamicTypes = new List<Type[]>();
        private DbContextTransaction _dbContextTransaction;

        public ComisionDbContext() : base("cnnstring")
        {
            Configuration.LazyLoadingEnabled = false;

            //Database.CommandTimeout = 180;

            //=== در وب اپلیکیشن باید از انتیتی فریم ورک غیر متصل استفاده کنیم و همچنین 
            //=== درصورتیکه ما تغییراتی روی داده‌ها نداشته باشیم و یا از روش‌های غیر متصل از موجودیت‌ها استفاده کنیم با استفاده از متد
            //=== AsNoTracking() در زمان و حافظه سیستم صرف جویی می‌کنیم در این حالت موجودیت‌های فراخوانی شده از دیتابیس در سیستم ردگیری
            //=== DbContext قرار نمی‌گیرند و  اگر  وضعیت آنها را بررسی کنیم در وضعیت Detached قرار دارند. 
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.AutoDetectChangesEnabled = false;
        }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<RolePermission> RolePermission { set; get; }
        public DbSet<Archive> Archive { set; get; }
        public DbSet<Authentication> Authentication { set; get; }
        public DbSet<BranchProvince> BranchProvince { set; get; }
        public DbSet<CentralOrganization> CentralOrganization { set; get; }
        public DbSet<College> College { set; get; }
        public DbSet<Commission> Commission { set; get; }
        public DbSet<CommissionSpecialEducation> CommissionSpecialEducation { set; get; }
        public DbSet<Council> Council { set; get; }
        public DbSet<EducationalGroup> EducationalGroup { set; get; }
        public DbSet<FieldofStudy> FieldofStudy { set; get; }
        public DbSet<MemberDetails> MemberDetails { set; get; }
        public DbSet<MemberMaster> MemberMaster { set; get; }
        public DbSet<OrganizationStructureName> OrganizationStructureName { set; get; }
        public DbSet<Permission> Permission { set; get; }
        public DbSet<Person> Person { set; get; }
        public DbSet<Personel> Personel { set; get; }
        public DbSet<Post> Post { set; get; }
        public DbSet<PostPerson> PostPerson { set; get; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Cartable> Cartable { get; set; }
        public DbSet<Signer> Signer { get; set; }
        public DbSet<SpecialEducation> SpecialEducation { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TextDefault> TextDefault { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }

        public DbSet<Slider> Sliders { get; set; }
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        #endregion

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries();
            // .Where(x => x.Entity is IAuditableEntity && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified)); 

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name + "-" + Thread.CurrentPrincipal.Identity.GetUserId();
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new ArchiveConfiguration());
            modelBuilder.Configurations.Add(new AttachmentConfiguration());
            modelBuilder.Configurations.Add(new AuthenticationConfiguration());
            modelBuilder.Configurations.Add(new BranchProvinceConfiguration());
            modelBuilder.Configurations.Add(new CartableConfiguration());
            modelBuilder.Configurations.Add(new CentralOrganizationConfiguration());
            modelBuilder.Configurations.Add(new CollegeConfiguration());
            modelBuilder.Configurations.Add(new CommissionConfiguration());
            modelBuilder.Configurations.Add(new CommissionSpecialEducationConfiguration());
            modelBuilder.Configurations.Add(new CouncilConfiguration());
            modelBuilder.Configurations.Add(new EducationalGroupConfiguration());
            modelBuilder.Configurations.Add(new FieldofStudyConfiguration());
            modelBuilder.Configurations.Add(new MemberDetailsConfiguration());
            modelBuilder.Configurations.Add(new MemberMasterConfiguration());
            modelBuilder.Configurations.Add(new OrganizationStructureNameConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new PersonelConfiguration());
            modelBuilder.Configurations.Add(new PostConfiguration());
            modelBuilder.Configurations.Add(new PostPersonConfiguration());
            modelBuilder.Configurations.Add(new ProfileConfiguration());
            modelBuilder.Configurations.Add(new RequestConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new RolePermissionConfiguration());
            modelBuilder.Configurations.Add(new SettingsConfiguration());
            modelBuilder.Configurations.Add(new SignerConfiguration());
            modelBuilder.Configurations.Add(new SpecialEducationConfiguration());
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new TextDefaultConfiguration());
            modelBuilder.Configurations.Add(new UniversityConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserRoleConfiguration());
            modelBuilder.Configurations.Add(new VoteConfiguration());
            modelBuilder.Configurations.Add(new UserPostConfiguration());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Person>().ToTable("Persons");
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public IList<T> GetRows<T>(string sql, params object[] parameters) where T : class
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public IList<T> GetRows<T>(string sql) where T : class
        {
            return Database.SqlQuery<T>(sql).ToList();
        }

        public void ExecCommand(string query)
        {
            Database.ExecuteSqlCommand(query);
        }

        public IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return ((DbSet<TEntity>)this.Set<TEntity>()).AddRange(entities);
        }

        public void ForceDatabaseInitialize()
        {
            Database.Initialize(force: true);
        }

        public void BeginTransaction()
        {
            _dbContextTransaction = this.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }

        public void ChangeEntityStates(EntityState state, object objectDepartment)
        {
            this.Entry(objectDepartment).State = state;
        }
    }
}
