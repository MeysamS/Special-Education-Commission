using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class ArchiveConfiguration: EntityTypeConfiguration<Archive>
    {
        public ArchiveConfiguration()
        {
            HasKey(k => k.RequestId);
            //ارتباط یک به صفر بین شی پایه اصلی  و فعالیت مورد نظر
            HasRequired(cu => cu.Request)
                .WithOptional(en => en.Archive)
                .WillCascadeOnDelete(false);
        }
    }
}
