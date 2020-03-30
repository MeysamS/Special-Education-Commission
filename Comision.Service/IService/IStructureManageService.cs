using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.Model;
using Newtonsoft.Json.Linq;

namespace Comision.Service.IService
{
    /// <summary>
    /// اینترفیس ساختار
    /// </summary>
    public interface IStructureManageService
    {
        Tuple<bool, string, IQueryable<EducationalGroup>> WhereEducationalGroup(
            Expression<Func<EducationalGroup, bool>> predicate);
        Tuple<bool, string> AddOrUpdateStructure(OrganizationStructureName structure, StateOperation stateOperation);
        Tuple<bool, string> AddOrUpdateCollege(College college, StateOperation stateOperation);
        Tuple<bool, string> AddOrUpdateEducationGroup(EducationalGroup educationalGroup, StateOperation stateOperation);
        Tuple<bool, string> AddOrUpdatefieldofStudy(FieldofStudy fieldofStudy, StateOperation stateOperation);
        Tuple<bool, string> AddorUpdateStructureComplete(StructureTreeModel model);

        Tuple<bool, string> Delete(StructureTreeModel model);

        IQueryable<OrganizationStructureName> GetStructure();
        Tuple<bool, string, List<SubStructureModel>> GetSubStructurebyPost(PostType postType, long levelId);
        List<JObject> GetStructureByTreeView(long univercityId);

        College FindCollegeInEducationalGroup(long id);
        EducationalGroup FindEducationalGroup(long id);
        FieldofStudy FindFieldofStudy(long id);
        Tuple<bool, string, List<FieldofStudy>> GetAllFieldofStudy();
        Tuple<bool, string, List<FieldofStudy>> GetAllFieldofStudy(long userId, int rowNumber);
    }
}
