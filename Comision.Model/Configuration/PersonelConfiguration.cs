using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{

    public class PersonelConfiguration : EntityTypeConfiguration<Personel>
    {
        public PersonelConfiguration()
        {
            HasKey(c => c.PersonId);
            Property(a => a.EmployeeNumber).IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(a => a.DateOfEmployeement).IsOptional();

            HasRequired(cu => cu.Person)
                .WithOptional(en => en.Personel)
                .WillCascadeOnDelete(false);
        }
    }
}
