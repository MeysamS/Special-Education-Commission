using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain;

namespace Comision.Model.Configuration
{
    public class AuthenticationConfiguration : EntityTypeConfiguration<Authentication>
    {
        public AuthenticationConfiguration()
        {
            HasKey(k => k.Id);

            Property(a => a.IdentityCode).IsRequired().IsUnicode(true).HasColumnType("nvarchar").HasMaxLength(50);
            Property(a => a.CentralOrganizationId).IsOptional();
            Property(a => a.BranchProvinceId).IsOptional();
            Property(a => a.UniversityId).IsOptional();

            HasOptional(x => x.CentralOrganization)
                .WithMany(x => x.Authentications)
                .HasForeignKey(x => x.CentralOrganizationId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.BranchProvince)
                    .WithMany(x => x.Authentications)
                    .HasForeignKey(x => x.BranchProvinceId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.University)
                    .WithMany(x => x.Authentications)
                    .HasForeignKey(x => x.UniversityId)
                    .WillCascadeOnDelete(false);
        }
    }
}
