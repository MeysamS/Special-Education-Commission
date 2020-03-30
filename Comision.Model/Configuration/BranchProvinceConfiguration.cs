using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class BranchProvinceConfiguration : EntityTypeConfiguration<BranchProvince>
    {
        public BranchProvinceConfiguration()
        {
            HasKey(k => k.Id);
            Property(a => a.Name).IsRequired().IsUnicode(true).HasColumnType("nvarchar").HasMaxLength(100);
            HasRequired(x => x.CentralOrganization)
                .WithMany(x => x.BranchProvinces)
                .HasForeignKey(x => x.CentralOrganizationId)
                .WillCascadeOnDelete(false);
        }
    }
}
