using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class CollegeConfiguration : EntityTypeConfiguration<College>
    {
        public CollegeConfiguration()
        {
            HasKey(k => k.Id);

            HasRequired(x => x.OrganizationStructureName)
                .WithMany(x => x.Colleges)
                .HasForeignKey(x => x.OrganizationStructureNameId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.University)
                .WithMany(x => x.Colleges)
                .HasForeignKey(x => x.UniversityId)
                .WillCascadeOnDelete(false);
        }
    }
}
