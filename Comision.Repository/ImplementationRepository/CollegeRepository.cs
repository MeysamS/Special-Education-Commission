using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class CollegeRepository : MainRepository<College>, ICollegeRepository
    {
        readonly IDbSet<College> _college;
        IUnitOfWork _unitOfWork;
        public CollegeRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _college = _unitOfWork.Set<College>();
        }
    }
}
