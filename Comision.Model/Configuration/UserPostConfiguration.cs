using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Configuration
{
    public class UserPostConfiguration : EntityTypeConfiguration<UserPost>
    {
        public UserPostConfiguration()
        {
            HasKey(k => k.Id);

            Property(a => a.CentralOrganizationId).IsOptional();
            Property(a => a.BranchProvinceId).IsOptional();
            Property(a => a.UniversityId).IsOptional();
            Property(a => a.CollegeId).IsOptional();
            Property(a => a.EducationalGroupId).IsOptional();

            HasRequired(x => x.User)
                .WithMany(x => x.UserPosts)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Post)
                .WithMany(x => x.UserPosts)
                .HasForeignKey(x => x.PostId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.CentralOrganization)
                .WithMany(x => x.UserPosts)
                .HasForeignKey(x => x.CentralOrganizationId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.BranchProvince)
                    .WithMany(x => x.UserPosts)
                    .HasForeignKey(x => x.BranchProvinceId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.University)
                    .WithMany(x => x.UserPosts)
                    .HasForeignKey(x => x.UniversityId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.College)
                .WithMany(x => x.UserPosts)
                .HasForeignKey(x => x.CollegeId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.EducationalGroup)
                    .WithMany(x => x.UserPosts)
                    .HasForeignKey(x => x.EducationalGroupId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.FieldofStudy)
                 .WithMany(x => x.UserPosts)
                 .HasForeignKey(x => x.FieldofStudyId)
                 .WillCascadeOnDelete(false);
        }
    }
}
