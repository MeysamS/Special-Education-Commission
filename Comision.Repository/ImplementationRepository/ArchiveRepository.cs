using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
  public  class ArchiveRepository : MainRepository<Archive>, IArchiveRepository
    {
        readonly IDbSet<Archive> _archive;
        IUnitOfWork _unitOfWork;
        public ArchiveRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _archive = _unitOfWork.Set<Archive>();
        }
    }
}
