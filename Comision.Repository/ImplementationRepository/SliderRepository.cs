using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class SliderRepository: MainRepository<Slider>, ISliderRepository
    {
        readonly IDbSet<Slider> _cartable;
        IUnitOfWork _unitOfWork;
        public SliderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cartable = _unitOfWork.Set<Slider>();
        }
    }
}
