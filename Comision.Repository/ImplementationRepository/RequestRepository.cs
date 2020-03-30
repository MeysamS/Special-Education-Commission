using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class RequestRepository : MainRepository<Request>, IRequestRepository
    {
        readonly IDbSet<Request> _request;
        IUnitOfWork _unitOfWork;
        public RequestRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _request = _unitOfWork.Set<Request>();
        }
    }
}
