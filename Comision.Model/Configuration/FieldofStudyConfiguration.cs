using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class FieldofStudyConfiguration : EntityTypeConfiguration<FieldofStudy>
    {
        public FieldofStudyConfiguration()
        {
            HasKey(k => k.Id);

            HasRequired(x => x.OrganizationStructureName)
                .WithMany(x => x.FieldofStudies)
                .HasForeignKey(x => x.OrganizationStructureNameId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.EducationalGroup)
                .WithMany(x => x.FieldofStudies)
                .HasForeignKey(x => x.EducationalGroupId)
                .WillCascadeOnDelete(false);
        }
    }
}
