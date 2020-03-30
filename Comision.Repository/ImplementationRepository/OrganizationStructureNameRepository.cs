using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class OrganizationStructureNameRepository : MainRepository<OrganizationStructureName>, IOrganizationStructureNameRepository
    {
        readonly IDbSet<OrganizationStructureName> _organizationStructureName;
        readonly IUnitOfWork _unitOfWork;
        public OrganizationStructureNameRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureName = _unitOfWork.Set<OrganizationStructureName>();
        }
    }
}
