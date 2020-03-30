using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class FieldofStudyRepository : MainRepository<FieldofStudy>, IFieldofStudyRepository
    {
        readonly IDbSet<FieldofStudy> _archive;
        IUnitOfWork _unitOfWork;
        public FieldofStudyRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _archive = _unitOfWork.Set<FieldofStudy>();
        }
    }
}
