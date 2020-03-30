using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Domain
{
    /// <summary>
    /// واحد مرکزی
    /// </summary>
    public class CentralOrganization : AuditableEntity<long>
    {
        /// <summary>
        /// آدرس واحد مرکزی
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// کد واحد مرکزی
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// نام واحد مرکزی
        /// </summary>
        [Index("IX_Name", IsUnique = true)]
        public string Name { get; set; }

        /// <summary>
        /// تلفن واحد مرکزی
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// لیست اشخاص
        /// </summary>
        public virtual ICollection<Person> Persons { get; set; }
        /// <summary>
        /// سمت های اشخاص  مرکز 
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }
        /// <summary>
        /// اختیارات کاربر در سطح مرکز
        /// </summary>
        public virtual ICollection<UserPost> UserPosts { get; set; }

        /// <summary>
        ///  نقش های تعریف شده  واحد مرکزی
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// لیست احراز هویت های تعریف شده
        /// </summary>
        public virtual ICollection<Authentication> Authentications { get; set; }

        /// <summary>
        /// لیست واحد های استانی مرکز
        /// </summary>
        public virtual ICollection<BranchProvince> BranchProvinces{ get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public CentralOrganization()
        { }
    }
}
