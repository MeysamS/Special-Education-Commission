using System.Collections.Generic;
using Comision.Model.Common;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Domain
{
    /// <summary>
    /// رشته تحصیلی
    /// </summary>
   public class FieldofStudy : AuditableEntity<long>
    {
        /// <summary>
        /// ایدی گروه اموزشی
        /// </summary>
        public long EducationalGroupId { get; set; }
      
        /// <summary>
        /// ایدی نام ساختار سامانی
        /// </summary>
        public long OrganizationStructureNameId { get; set; }

        /// <summary>
        /// شی گروه آموزشی
        /// </summary>
        public virtual EducationalGroup EducationalGroup { get; set; }
        
        /// <summary>
        /// شی نام ساختار سازمانی
        /// </summary>
        public virtual OrganizationStructureName OrganizationStructureName { get; set; }
       
        /// <summary>
        /// لیست دانشجویان
        /// </summary>
        public virtual ICollection<Student> Students { get; set; }

        /// <summary>
        ///لیست اشخاص با سمت هایشان در این رشته
        /// </summary>
        public virtual ICollection<PostPerson> PostPersons { get; set; }

        /// <summary>
        /// اختیارات کاربر در سطح رشته
        /// </summary>
        public virtual ICollection<UserPost> UserPosts { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public FieldofStudy()
        { }
    }
}
