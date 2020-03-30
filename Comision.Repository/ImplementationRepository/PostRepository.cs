using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class PostRepository : MainRepository<Post>, IPostRepository
    {
        readonly IDbSet<Post> _post;
        IUnitOfWork _unitOfWork;
        public PostRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _post = _unitOfWork.Set<Post>();
        }
    }
}
