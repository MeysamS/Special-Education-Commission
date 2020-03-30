using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class OrganizationStructureNameConfiguration : EntityTypeConfiguration<OrganizationStructureName>
    {
        public OrganizationStructureNameConfiguration()
        {
            HasKey(c => c.Id);
            Property(a => a.Name).IsRequired().IsUnicode(true).HasColumnType("nvarchar").HasMaxLength(200);
        }
    }
}
