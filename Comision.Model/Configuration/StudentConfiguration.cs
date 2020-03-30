using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{

    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            HasKey(c => c.PersonId);

            //ارتباط دانشجو با رای شخص - یک به صفر
            HasRequired(cu => cu.Person)
                .WithOptional(en => en.Student)
                .WillCascadeOnDelete(false);

            //ارتباط بین رشته تحصلی  با دانشجو - یک به چند
            HasRequired(x => x.FieldofStudy)
              .WithMany(x => x.Students)
              .HasForeignKey(x => x.FieldofStudyId)
              .WillCascadeOnDelete(false);
        }
    }
}
