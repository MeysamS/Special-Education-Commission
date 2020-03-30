using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Domain
{
    /// <summary>
    /// واحد مرکز استان
    /// </summary>
    public class BranchProvince : AuditableEntity<long>
    {
        /// <summary>
        /// آدرس واحد مرکز استان
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// کد واحد مرکز استان
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// نام واحد مرکز استان
        /// </summary>
        [Index("IX_Name", IsUnique = true)]
        public string Name { get; set; }
       
        /// <summary>
        /// تلفن واحد مرکز استان
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// ایدی مرکز استان
        /// </summary>
        public long CentralOrganizationId { get; set; }

        /// <summary>
        /// شی مرکز استان
        /// </summary>
        public virtual CentralOrganization CentralOrganization { get; set; }

        /// <summary>
        /// لیست اشخاص
        /// </summary>
        public virtual ICollection<Person> Persons { get; set; }
        /// <summary>
        /// سمت های اشخاص این شعبه 
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }

        /// <summary>
        /// اختیارات کاربر در سطح شعبه
        /// </summary>
        public virtual ICollection<UserPost> UserPosts { get; set; }

        /// <summary>
        ///  نقش های تعریف شده  واحد سازمان
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// لیست دانشگاهای واحد مرکز استان
        /// </summary>
        public virtual ICollection<University> Universities { get; set; }

        /// <summary>
        /// لیست احراز هویت های تعریف شده
        /// </summary>
        public virtual ICollection<Authentication> Authentications { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public BranchProvince()
        {

        }

    }
}
