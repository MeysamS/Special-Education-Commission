using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class SpecialEducationRepository : MainRepository<SpecialEducation>, ISpecialEducationRepository
    {
        readonly IDbSet<SpecialEducation> _specialEducation;
        readonly IUnitOfWork _unitOfWork;
        public SpecialEducationRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _specialEducation = _unitOfWork.Set<SpecialEducation>();
        }
    }
}
