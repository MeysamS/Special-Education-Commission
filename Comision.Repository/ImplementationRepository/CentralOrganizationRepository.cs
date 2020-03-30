using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class CentralOrganizationRepository : MainRepository<CentralOrganization>, ICentralOrganizationRepository
    {
        readonly IDbSet<CentralOrganization> _centralOrganization;
        IUnitOfWork _unitOfWork;
        public CentralOrganizationRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _centralOrganization = _unitOfWork.Set<CentralOrganization>();
        }        
    }

 
}
