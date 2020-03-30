using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// نام ساختار سازمانی
    /// </summary>
    public class OrganizationStructureName : AuditableEntity<long>
    {
        /// <summary>
        /// نام ساختار
        /// </summary>
        [Index("IX_OrganizationStructureName", IsUnique = true)]
        public string Name { get; set; }

        /// <summary>
        /// نوع ساختار
        /// </summary>
        public StructureType StructureType { get; set; }

        /// <summary>
        /// لیست دانشکده ها
        /// </summary>
        public virtual ICollection<College> Colleges { get; set; }

        /// <summary>
        /// لیست گروه های آموزشی
        /// </summary>
        public virtual ICollection<EducationalGroup> EducationalGroups { get; set; }
      
        /// <summary>
        /// لیست رشته های تحصیلی
        /// </summary>
        public virtual ICollection<FieldofStudy> FieldofStudies { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public OrganizationStructureName()
        { }
    }
}
