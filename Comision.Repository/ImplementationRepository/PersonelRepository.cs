using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class PersonelRepository : MainRepository<Personel>, IPersonelRepository
    {
        readonly IDbSet<Personel> _personel;
        IUnitOfWork _unitOfWork;
        public PersonelRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _personel = _unitOfWork.Set<Personel>();
        }
    }
}
