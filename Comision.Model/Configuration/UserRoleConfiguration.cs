using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Configuration
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            HasKey(k => new { k.UserId, k.RoleId });

            HasRequired(x => x.User)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .WillCascadeOnDelete(false);
        }
    }
}
