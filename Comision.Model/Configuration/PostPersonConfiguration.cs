using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class PostPersonConfiguration : EntityTypeConfiguration<PostPerson>
    {
        public PostPersonConfiguration()
        {
            HasKey(k => k.Id);

            Property(a => a.CentralOrganizationId).IsOptional();
            Property(a => a.BranchProvinceId).IsOptional();
            Property(a => a.UniversityId).IsOptional();
            Property(a => a.CollegeId).IsOptional();
            Property(a => a.EducationalGroupId).IsOptional();
            
            HasRequired(x => x.Person)
                .WithMany(x => x.PostPersons)
                .HasForeignKey(x => x.PersonId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Post)
                .WithMany(x => x.PostPersons)
                .HasForeignKey(x => x.PostId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.CentralOrganization)
                .WithMany(x => x.PostPersons)
                .HasForeignKey(x => x.CentralOrganizationId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.BranchProvince)
                    .WithMany(x => x.PostPersons)
                    .HasForeignKey(x => x.BranchProvinceId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.University)
                    .WithMany(x => x.PostPersons)
                    .HasForeignKey(x => x.UniversityId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.College)
                .WithMany(x => x.PostPersons)
                .HasForeignKey(x => x.CollegeId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.EducationalGroup)
                    .WithMany(x => x.PostPersons)
                    .HasForeignKey(x => x.EducationalGroupId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.FieldofStudy)
                   .WithMany(x => x.PostPersons)
                   .HasForeignKey(x => x.FieldofStudyId)
                   .WillCascadeOnDelete(false);
        }
    }
}
