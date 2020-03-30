using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Configuration
{
    public class ProfileConfiguration : EntityTypeConfiguration<Profile>
    {
        public ProfileConfiguration()
        {
            HasKey(c => c.PersonId);

            Property(a => a.NationalCode).IsRequired().IsUnicode(true).HasColumnType("nvarchar").HasMaxLength(50);
            Property(a => a.BrithDate).IsOptional();

            //ارتباط یک به صفر بین شی پایه اصلی  و فعالیت مورد نظر
            HasRequired(cu => cu.Person)
                .WithOptional(en => en.Profile)
                .WillCascadeOnDelete(false);
        }
    }
}
