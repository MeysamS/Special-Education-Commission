using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class EducationalGroupRepository : MainRepository<EducationalGroup>, IEducationalGroupRepository
    {
        readonly IDbSet<EducationalGroup> _educationalGroup;
        IUnitOfWork _unitOfWork;
        public EducationalGroupRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _educationalGroup = _unitOfWork.Set<EducationalGroup>();
        }
    }
}
