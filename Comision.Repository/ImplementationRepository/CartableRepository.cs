using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class CartableRepository : MainRepository<Cartable>, ICartableRepository
    {
        readonly IDbSet<Cartable> _cartable;
        IUnitOfWork _unitOfWork;
        public CartableRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cartable = _unitOfWork.Set<Cartable>();
        }
    }
}
