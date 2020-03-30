using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class UniversityRepository : MainRepository<University>, IUniversityRepository
    {
        readonly IDbSet<University> _university;
        IUnitOfWork _unitOfWork;
        public UniversityRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _university = _unitOfWork.Set<University>();
        }
        
    }
}
