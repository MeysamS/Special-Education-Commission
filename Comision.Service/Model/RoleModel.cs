using System.Collections.Generic;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class RoleModel
    {
        public long? Id { get; set; }
        public long? CentralOrganizationId { get; set; }        
        public long? BranchProvinceId { get; set; }        
        public long? UniversityId { get; set; }  
              
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        public string NameFa { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public RoleType RoleType { get; set; }
        public string XmlRoleAccess { get; set; }
        public List<ControllerModel> SelectedControllers { get; set; }
    }
}
