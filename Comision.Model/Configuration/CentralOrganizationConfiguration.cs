using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
   public class CentralOrganizationConfiguration : EntityTypeConfiguration<CentralOrganization>
    {
        public CentralOrganizationConfiguration()
        {
            HasKey(c => c.Id);
            Property(a => a.Name).IsRequired().IsUnicode(true).HasColumnType("nvarchar").HasMaxLength(100);
        }
    }
}
