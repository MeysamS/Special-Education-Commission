using System.Data.Entity.ModelConfiguration;
using Comision.Model.Domain.UserDomain;

namespace Comision.Model.Configuration
{
   public class RolePermissionConfiguration : EntityTypeConfiguration<RolePermission>
    {
        public RolePermissionConfiguration()
        {
            HasKey(k => new { k.RoleId, k.PermissionId });
            
        }
    }
}
