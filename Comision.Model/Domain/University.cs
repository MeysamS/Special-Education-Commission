
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Domain
{
    /// <summary>
    /// <span dir="rtl">دانشگاه</span>
    /// </summary>
    public class University : AuditableEntity<long>, IEnumerable
    {
        /// <summary>
        /// ایدی واحد سازمانی
        /// </summary>
        public long BranchProvinceId { get; set; }
        /// <summary>
        /// شی واحد سازمانی
        /// </summary>
        public virtual BranchProvince BranchProvince { get; set; }        

        /// <summary>
        /// کد دانشگاه
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// نام دانشگاه
        /// </summary>
        [Index("IX_Name", IsUnique = true)]
        public string Name { get; set; }
       /// <summary>
       /// تلفن
       /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// لوگو
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// تنظیمات 
        /// </summary>
        public virtual Settings Settings { get; set; }
        /// <summary>
        /// لیست دانشکده ها
        /// </summary>
        public virtual ICollection<College> Colleges { get; set; }
        /// <summary>
        /// لیست مستر برای امضا
        /// </summary>
        public virtual ICollection<MemberMaster> MemberMasters { get; set; }
        /// <summary>
        /// لیست اشخاص
        /// </summary>
        public virtual ICollection<Person> Persons { get; set; }
        /// <summary>
        /// لیست سمت های اشخاص
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }

        /// <summary>
        /// اختیارات کاربر در سطح دانشگاه
        /// </summary>
        public virtual ICollection<UserPost> UserPosts { get; set; }
        /// <summary>
        /// لیست نقش ها
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
        /// <summary>
        /// لیست احرازهویت ها
        /// </summary>
        public virtual ICollection<Authentication> Authentications { get; set; }

        /// <summary>
        /// لیست متون پیش فرض رای کمسیون و شورا
        /// </summary>
        public virtual ICollection<TextDefault> TextDefaults { get; set; }
        public University() { }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }//end University
}