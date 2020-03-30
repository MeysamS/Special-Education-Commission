using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// <span dir="rtl">„Ê«—œ Œ«’ ¬„Ê“‘Ì</span>
    /// </summary>
    public class SpecialEducation : AuditableEntity<long>
    {
        [Index("IX_Name", IsUnique = true)]
        public string Name { get; set; }
        public virtual ICollection<CommissionSpecialEducation> CommissionSpecialEducations { get; set; }

        public SpecialEducation() { }

    }//end SpecialEducation
}