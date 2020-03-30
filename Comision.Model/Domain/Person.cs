using System.Collections.Generic;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Domain
{
    /// <summary>
    /// کلاس شخص
    /// </summary>
    public class Person : AuditableEntity<long>
    {
        /// <summary>
        /// فعال/غیر فعال
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// ایدی مرکز
        /// </summary>
        public long? CentralOrganizationId { get; set; }
        public CentralOrganization CentralOrganization { get; set; }

        /// <summary>
        /// ایدی واحد سازمانی
        /// </summary>
        public long? BranchProvinceId { get; set; }
        public BranchProvince BranchProvince { get; set; }
        /// <summary>
        /// ایدی دانشگاه
        /// </summary>
        public long? UniversityId { get; set; }
        public University University { get; set; }

        /// <summary>
        /// شی پروفایل
        /// </summary>
        public virtual Profile Profile { get; set; }

        /// <summary>
        /// شی کاربر
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// شی پرسنل
        /// </summary>
        public virtual Personel Personel { get; set; }

        /// <summary>
        /// شی دانشجو
        /// </summary>
        public virtual Student Student { get; set; }

        /// <summary>
        /// لیست پست های شخص
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }

        /// <summary>
        /// لیست درخواست های تایید کرده است
        /// </summary>
        public virtual ICollection<Cartable> Cartables { get; set; }

        /// <summary>
        /// لیست کارتابل های تایید کرده از طرف 
        /// </summary>
        public virtual ICollection<Cartable> CartablesBehalfof { get; set; }

        /// <summary>
        /// لیست درخواست ها
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; }

        /// <summary>
        /// لیست رای صادر شده توسط این شخص
        /// </summary>
        public virtual ICollection<Vote> Votes { get; set; }

        /// <summary>
        /// لیست اعضای کمیسون و شورا
        /// </summary>
        public virtual ICollection<MemberDetails> MemberDetails { get; set; }

        /// <summary>
        /// سازنده اولیه 
        /// </summary>
        public Person()
        { }
    }
}
