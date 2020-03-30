using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
   public  class CommissionSpecialEducationConfiguration : EntityTypeConfiguration<CommissionSpecialEducation>
    {
        public CommissionSpecialEducationConfiguration()
        {
            HasKey(k => new { k.CommissionId, k.SpecialEducationId });

            HasRequired(x => x.Commission)
                .WithMany(x => x.CommissionSpecialEducations)
                .HasForeignKey(x => x.CommissionId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.SpecialEducation)
                .WithMany(x => x.CommissionSpecialEducations)
                .HasForeignKey(x => x.SpecialEducationId)
                .WillCascadeOnDelete(false);
        }
    }
}
