using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class AuthenticationModel
    {
        public long? Id { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string IdentityCode { get; set; }
        public long? CentralOrganizationId { get; set; }
        public long? BranchProvinceId { get; set; }
        public long? UniversityId { get; set; }
        public virtual AuthenticationType AuthenticationType { get; set; }
    }

    public class AuthenticationComparer : IEqualityComparer<AuthenticationModel>
    {
        public bool Equals(AuthenticationModel x, AuthenticationModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.IdentityCode == y.IdentityCode;
        }

        public int GetHashCode(AuthenticationModel authenticationModel)
        {
            if (ReferenceEquals(authenticationModel, null))
                return 0;

            int hashIdentityCode = authenticationModel.IdentityCode.GetHashCode();

            return hashIdentityCode;
        }
    }
}
