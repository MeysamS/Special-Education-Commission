using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class CommissionConfiguration : EntityTypeConfiguration<Commission>
    {
        public CommissionConfiguration()
        {
            HasKey(c => c.RequestId);
            
            HasRequired(cu => cu.Request)
                .WithOptional(en => en.Commission)
                .WillCascadeOnDelete(false);
        }
    }
}
