using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Configuration
{
  public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            HasKey(k => k.Id);

            Property(a => a.CentralOrganizationId).IsOptional();
            Property(a => a.BranchProvinceId).IsOptional();
            Property(a => a.UniversityId).IsOptional();
            Property(a => a.Name).IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(a => a.NameFa).IsRequired().IsUnicode(true).HasColumnType("nvarchar").HasMaxLength(100);

            Property(c => c.XmlRoleAccess).HasColumnType("xml");
            Ignore(c => c.XmlValueWrapper);

            HasOptional(x => x.CentralOrganization)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.CentralOrganizationId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.BranchProvince)
                    .WithMany(x => x.Roles)
                    .HasForeignKey(x => x.BranchProvinceId)
                    .WillCascadeOnDelete(false);

            HasOptional(x => x.University)
                    .WithMany(x => x.Roles)
                    .HasForeignKey(x => x.UniversityId)
                    .WillCascadeOnDelete(false);
        }
    }
}
