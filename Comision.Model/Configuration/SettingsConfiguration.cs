using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class SettingsConfiguration : EntityTypeConfiguration<Settings>
    {
        public SettingsConfiguration()
        {
            HasKey(c => c.UniversityId);            

            //ارتباط یک به صفر بین شی پایه اصلی  و فعالیت مورد نظر
            HasRequired(cu => cu.University)
                .WithOptional(en => en.Settings)
                .WillCascadeOnDelete(false);
        }
    }
}
