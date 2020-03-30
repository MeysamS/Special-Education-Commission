using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class PersonRepository : MainRepository<Person>, IPersonRepository
    {
        readonly IDbSet<Person> _person;
        IUnitOfWork _unitOfWork;
        public PersonRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _person = _unitOfWork.Set<Person>();
        }
    }
}
