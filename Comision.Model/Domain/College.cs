using System.Collections.Generic;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Domain
{
    /// <summary>
    /// گروه  آموزشی
    /// </summary>
    public class College : AuditableEntity<long>
    {
        /// <summary>
        /// ایدی نام ساختار 
        /// </summary>
        public long OrganizationStructureNameId { get; set; }

        /// <summary>
        /// ایدی دانشگاه
        /// </summary>
        public long UniversityId { get; set; }
        /// <summary>
        /// شی نام ساختار سازمان
        /// </summary>
        public virtual OrganizationStructureName OrganizationStructureName { get; set; }

        /// <summary>
        /// شی دانشگاه
        /// </summary>
        public virtual University University { get; set; }

        /// <summary>
        /// لیست گروه های آموزشی 
        /// </summary>
        public virtual ICollection<EducationalGroup> EducationalGroups { get; set; }

        /// <summary>
        /// لیست سمت های اشخاص این گروه اموزشی
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }

        /// <summary>
        /// اختیارات کاربر در سطح دانشکده
        /// </summary>
        public virtual ICollection<UserPost> UserPosts { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public College()
        { }
    }
}
