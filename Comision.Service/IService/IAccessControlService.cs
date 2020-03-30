using System.Collections.Generic;
using Comision.Model.Domain.UserDomain;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface IAccessControlService
    {
        List<ControllerModel> ConvertXmlToListControllers(string xml);
        string ConvertListControllerToXml(List<ControllerModel> controllers);
        List<string> GetUserPermissions(long userId);
        List<Role> GetUserRoles(long userId);
        bool HasPermission(string xmlPermission, string roleNameEn);

    }
}
