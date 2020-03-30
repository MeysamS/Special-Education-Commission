using System.ComponentModel.DataAnnotations;
using Comision.Model.Domain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class StudentModel
    {
        public virtual long PersonId { get; set; }

        [Display(Name = "رشته تحصیلی")]
        public virtual long FieldofStudyId { get; set; }

        [Display(Name = "مقطع تحصیلی")]
        public virtual Grade Grade { get; set; }

        [Display(Name = "وضعیت خدمت سربازی")]
        public virtual MilitaryServiceStatus MilitaryServiceStatus { get; set; }

        [Display(Name = "شماره دانشجویی")]
        public virtual long StudentNumber { get; set; }

        /// <summary>
        /// فعال/غیر فعال
        /// </summary>
        [Display(Name = "فعال")]
        public virtual bool Active { get; set; }
        public StudentModel() { }
        public StudentModel(Student student)
        {
            if (student == null)
                return;
            PersonId = student.PersonId;
            FieldofStudyId = student.FieldofStudyId;
            MilitaryServiceStatus = student.MilitaryServiceStatus;
            Grade = student.Grade;
            StudentNumber = student.StudentNumber;
            Active = student.Active;
        }
    }
}
