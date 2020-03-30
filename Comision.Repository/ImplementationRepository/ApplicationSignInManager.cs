using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Comision.Repository.ImplementationRepository
{
    public class ApplicationSignInManager :
        SignInManager<User, long>, IApplicationSignInManager
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public ApplicationSignInManager(ApplicationUserManager userManager,
                                        IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }
    }
}