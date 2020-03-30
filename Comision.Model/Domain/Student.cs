using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// <span dir="rtl">دانشجو</span>
    /// </summary>
    public class Student: Auditable
    {
        /// <summary>
        /// <span dir="rtl">کلید اصلی و کلید خارجی</span>
        /// </summary>
        public long PersonId { get; set; }
        public virtual Person Person { get; set; }
        public long FieldofStudyId { get; set; }
        public virtual FieldofStudy FieldofStudy { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual MilitaryServiceStatus MilitaryServiceStatus { get; set; }

        [Index("IX_StudentNumber", IsUnique = true)]
        public long StudentNumber { get; set; }

        /// <summary>
        /// فعال/غیر فعال
        /// </summary>
        public virtual bool Active { get; set; }

        public Student() { }

    }//end Student
}