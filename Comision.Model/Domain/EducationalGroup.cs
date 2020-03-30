using System.Collections.Generic;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Domain
{
    /// <summary>
    /// گروه اموزشی
    /// </summary>
    public class EducationalGroup : AuditableEntity<long>
    {
        /// <summary>
        /// کد گروه آموزشی
        /// </summary>
        public string Code { get; set; }
       
        /// <summary>
        /// ایدی نام ساختار سازمانی
        /// </summary>
        public long OrganizationStructureNameId { get; set; }
       
        /// <summary>
        /// ایدی دانشکده
        /// </summary>
        public long CollegeId { get; set; }
       
        /// <summary>
        /// شی دانشگاه
        /// </summary>
        public virtual College College { get; set; }

        /// <summary>
        /// شی نام ساختار سازمانی
        /// </summary>
        public virtual OrganizationStructureName OrganizationStructureName { get; set; }

        /// <summary>
        /// لیست سمت های اشخاص
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }

        /// <summary>
        /// اختیارات کاربر در سطح گروه آموزشی
        /// </summary>
        public virtual ICollection<UserPost> UserPosts { get; set; }
        public virtual ICollection<FieldofStudy> FieldofStudies { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public EducationalGroup()
        {

        }
    }
}
