using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// احراز هویت
    /// در این کلاس کد های شناسایی قرار می گیرند
    /// </summary>
    public class Authentication : AuditableEntity<long>
    {
        /// <summary>
        /// نوع احراز هویت
        /// </summary>
        public AuthenticationType AuthenticationType { get; set; }

        /// <summary>
        /// ایدی سازمان مرکزی
        /// </summary>
        public long? CentralOrganizationId { get; set; }
        public virtual CentralOrganization CentralOrganization { get; set; }

        /// <summary>
        /// ایدی واحد مرکز استان
        /// </summary>
        public long? BranchProvinceId { get; set; }
        public virtual BranchProvince BranchProvince { get; set; }

        /// <summary>
        /// ایدی دانشگاه
        /// </summary>
        public long? UniversityId { get; set; }
        public virtual University University { get; set; }

        /// <summary>
        /// کد شناسایی که برابر با کد ملی اشخاص می باشد
        /// </summary>
        [Index("IX_IdentityCode", IsUnique = true)]
        public string IdentityCode { get; set; }

        /// <summary>
        /// این فیلد مشخص می کند که این کد شناسایی مورد نظر مربوط به شخصی است که قبلا در سیستم وجود داشت
        /// </summary>
        public bool ExistinProfile{ get; set; }

        /// <summary>
        /// لیست کاربرانی که از این کد استفاده نمده اند
        /// البته در اصل یک کاربر از هر کد استفاده میکند چون ارتباط ممکن نبود
        /// لیست گذاشته ایم 
        /// و برای بازگشایی نیاز است فقط اندیس صفر برداشته شود
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public Authentication()
        {
        }
        public Authentication(long levelId, AuthenticationType authenticationType, string identityCode)
        {
            IdentityCode = identityCode;
            AuthenticationType = authenticationType;
            if(authenticationType == AuthenticationType.AdminCentral)
                CentralOrganizationId = levelId;
            else if (authenticationType == AuthenticationType.AdminBranch)
                BranchProvinceId = levelId;
            else
                UniversityId = levelId;
        }
    }
}
