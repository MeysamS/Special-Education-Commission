using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class CouncilConfiguration : EntityTypeConfiguration<Council>
    {
        public CouncilConfiguration()
        {
            HasKey(c => c.RequestId);
            
            HasRequired(cu => cu.Request)
                .WithOptional(en => en.Council)
                .WillCascadeOnDelete(false);
        }
    }
}
