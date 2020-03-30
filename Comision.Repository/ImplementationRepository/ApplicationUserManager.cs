using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;

namespace Comision.Repository.ImplementationRepository
{
    public class ApplicationUserManager
        : UserManager<User, long>, IApplicationUserManager
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IIdentityMessageService _emailService;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IIdentityMessageService _smsService;
        private readonly IUserRepository _userRepository;
        private readonly IUserStore<User, long> _store;

        public ApplicationUserManager(IUserStore<User, long> store, IUserRepository userRepository,
            IApplicationRoleManager roleManager, IDataProtectionProvider dataProtectionProvider,
            IIdentityMessageService smsService, IIdentityMessageService emailService) : base(store)
        {
            _store = store;
            _roleManager = roleManager;
            _dataProtectionProvider = dataProtectionProvider;
            _smsService = smsService;
            _emailService = emailService;
            _userRepository = userRepository;
            CreateApplicationUserManager();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User applicationUser)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Email", applicationUser.Email));
            if (applicationUser.Person.Profile != null)
                userIdentity.AddClaim(new Claim("FullName", applicationUser.Person.Profile.FullName));
            return userIdentity;
        }

        public async Task<bool> HasPassword(long userId)
        {
            var user = await FindByIdAsync(userId);
            return user != null && user.PasswordHash != null;
        }

        public async Task<bool> HasPhoneNumber(long userId)
        {
            var user = await FindByIdAsync(userId);
            return user != null && user.PhoneNumber != null;
        }

        public async Task<User> FindWithProfileByIdAsync(long userId)
        {
            User user = await _userRepository.Where(x => x.Id == userId).Include(x => x.Person).Include(x => x.Person.Profile).FirstOrDefaultAsync();
            return user;
        }

        public Func<CookieValidateIdentityContext, Task> OnValidateIdentity()
        {
            return SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, long>(
                         validateInterval: TimeSpan.FromMinutes(30),
                         regenerateIdentityCallback: generateUserIdentityAsync,
                         getUserIdCallback: (id) => (int.Parse(id.GetUserId())));
        }

        public void SeedDatabase()
        {
            //const string name = "admin@example.com";
            //const string password = "Admin@123456";
            //const string roleName = "Admin";

            ////Create Role Admin if it does not exist
            //var role = _roleManager.FindRoleByName(roleName);
            //if (role == null)
            //{
            //    role = new CustomRole(roleName,"مدیر سایت");
            //    var roleresult = _roleManager.CreateRole(role);
            //}

            //var user = this.FindByName(name);
            //if (user == null)
            //{
            //    user = new User { UserName = name, Email = name };
            //    var result = this.Create(user, password);
            //    result = this.SetLockoutEnabled(user.Id, false);
            //}

            //// Add user admin to Role Admin if not already added
            //var rolesForUser = this.GetRoles(user.Id);
            //if (!rolesForUser.Contains(role.Name))
            //{
            //    var result = this.AddToRole(user.Id, role.Name);
            //}
        }

        private void CreateApplicationUserManager()
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<User, long>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            this.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User, long>
            {
                MessageFormat = "Your security code is: {0}"
            });
            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User, long>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            this.EmailService = _emailService;
            this.SmsService = _smsService;

            if (_dataProtectionProvider == null) return;
            var dataProtector = _dataProtectionProvider.Create("ASP.NET Identity");
            this.UserTokenProvider = new DataProtectorTokenProvider<User, long>(dataProtector);
        }

        private async Task<ClaimsIdentity> generateUserIdentityAsync(ApplicationUserManager manager, User applicationUser)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}