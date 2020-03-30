using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class EducationalGroupConfiguration : EntityTypeConfiguration<EducationalGroup>
    {
        public EducationalGroupConfiguration()
        {
            HasKey(k => k.Id);

            HasRequired(x => x.OrganizationStructureName)
                .WithMany(x => x.EducationalGroups)
                .HasForeignKey(x => x.OrganizationStructureNameId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.College)
                .WithMany(x => x.EducationalGroups)
                .HasForeignKey(x => x.CollegeId)
                .WillCascadeOnDelete(false);
        }
    }
}
