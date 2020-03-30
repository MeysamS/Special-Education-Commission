using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class AuthenticationRepository : MainRepository<Authentication>, IAuthenticationRepository
    {
        readonly IDbSet<Authentication> _authentication;

        public AuthenticationRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _authentication = unitOfWork.Set<Authentication>();
        }
    }
}
