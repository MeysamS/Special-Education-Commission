using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class PostPersonRepository : MainRepository<PostPerson>, IPostPersonRepository
    {
        readonly IDbSet<PostPerson> _postPerson;
        IUnitOfWork _unitOfWork;
        public PostPersonRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _postPerson = _unitOfWork.Set<PostPerson>();
        }
    }
}
