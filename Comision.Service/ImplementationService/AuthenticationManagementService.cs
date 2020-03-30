using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Comision.Service.ImplementationService
{
    public class AuthenticationManagementService : IAuthenticationManagementService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICentralOrganizationRepository _centralOrganizationRepository;
        private readonly IBranchProvinceRepository _branchProvinceRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IUserRepository _userRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IPostPersonRepository _postPersonRepository;
        private readonly ISignerRepository _signerRepository;
        private readonly IProfileRepository _profileRepository;

        public AuthenticationManagementService(IUnitOfWork uow, ICentralOrganizationRepository centralOrganizationRepository,
            IBranchProvinceRepository branchProvinceRepository, IUniversityRepository universityRepository,
            IPersonRepository personRepository, IAuthenticationRepository authenticationRepository,
            IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IApplicationUserManager userManager,
            IApplicationRoleManager roleManager, IUserLoginRepository userLoginRepository, IAuthenticationManager authenticationManager,
            IUserRepository userRepository, IProfileRepository profileRepository, IPostPersonRepository postPersonRepository,
            ISignerRepository signerRepository, ISettingsRepository settingsRepository)
        {
            _unitOfWork = uow;
            _centralOrganizationRepository = centralOrganizationRepository;
            _branchProvinceRepository = branchProvinceRepository;
            _universityRepository = universityRepository;
            _authenticationRepository = authenticationRepository;
            _personRepository = personRepository;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userManager = userManager;
            _userLoginRepository = userLoginRepository;
            _authenticationManager = authenticationManager;
            _userRepository = userRepository;
            _settingsRepository = settingsRepository;
            _postPersonRepository = postPersonRepository;
            _signerRepository = signerRepository;
            _profileRepository = profileRepository;
        }

        /// <summary>
        /// این متد رکورد سازمان مرکزی را به همراه احراز هویت ، شخص ، کاربر و نقش مدیر سازمان را ثبت می کند
        /// و نقش را به کاربر مربوطه انتساب می دهد
        /// </summary>
        /// <param name="modelRegisterProgram"></param>
        /// <returns></returns>
        public bool RegisterProgramCentralOrganization(RegisterProgramCentralOrganizationModel modelRegisterProgram)
        {
            try
            {
                //ثبت رکورد سازمان مرکزی
                CentralOrganization centralOrganization = new CentralOrganization
                {
                    Name = modelRegisterProgram.OrganName,
                    Address = modelRegisterProgram.Address,
                    Code = modelRegisterProgram.Code,
                    Phone = modelRegisterProgram.Phone
                };
                _centralOrganizationRepository.Add(centralOrganization);

                AddUserAdminAutomatic(centralOrganization.Id, AuthenticationType.AdminCentral, RoleType.AdminCentral, modelRegisterProgram);

                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// این متد رکورد سازمان مرکزی را به همراه واحد مرکز استان ، احراز هویت ، شخص ، کاربر 
        /// و نقش مدیر واحد مرکز استان را ثبت می کند و نقش را به کاربر مربوطه انتساب می دهد
        /// </summary>
        /// <param name="modelRegisterProgram"></param>
        /// <returns></returns>
        public bool RegisterProgramBranchProvince(RegisterProgramBranchProvinceModel modelRegisterProgram)
        {
            try
            {
                //ثبت رکورد سازمان مرکزی
                CentralOrganization centralOrganization = new CentralOrganization
                {
                    Name = "سازمان مرکزی"
                };
                _centralOrganizationRepository.Add(centralOrganization);

                BaseRegisterProgramModel baseRegisterProgramModel = new BaseRegisterProgramModel
                {
                    RoleType = RoleType.AdminCentral,
                    Name = "مدیر سازمان",
                    NationalCode = "1111111111",
                    Password = "admincentral",
                    OrganName = "سازمان مرکزی",
                    UserName = "admincentral@mail.com",
                    LicenceCode = "1111111111"
                };
                AddUserAdminAutomatic(centralOrganization.Id, AuthenticationType.AdminCentral, RoleType.AdminCentral, baseRegisterProgramModel);

                //ثبت رکورد واحد مرکز استان
                BranchProvince branchProvince = new BranchProvince
                {
                    CentralOrganizationId = centralOrganization.Id,
                    Name = modelRegisterProgram.OrganName,
                    Address = modelRegisterProgram.Address,
                    Code = modelRegisterProgram.Code,
                    Phone = modelRegisterProgram.Phone
                };
                _branchProvinceRepository.Add(branchProvince);

                AddUserAdminAutomatic(branchProvince.Id, AuthenticationType.AdminBranch, RoleType.AdminBranch, modelRegisterProgram);

                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// این متد رکورد سازمان مرکزی را به همراه واحد مرکز استان ، دانشگاه ، احراز هویت ، شخص ، کاربر 
        /// و نقش مدیر دانشگاه را ثبت می کند و نقش را به کاربر مربوطه انتساب می دهد
        /// </summary>
        /// <param name="modelRegisterProgram"></param>
        /// <returns></returns>
        public bool RegisterProgramUniversity(RegisterProgramUniversityModel modelRegisterProgram)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                //ثبت رکورد سازمان مرکزی
                var centralOrganization = new CentralOrganization
                {
                    Name = "سازمان مرکزی"
                };
                _centralOrganizationRepository.Add(centralOrganization);

                //var baseRegisterProgramModel = new BaseRegisterProgramModel
                //{
                //    RoleType = RoleType.AdminCentral,
                //    Name = "مدیر سازمان",
                //    NationalCode = "1111111111",
                //    Password = "admincentral",
                //    OrganName = "سازمان مرکزی",
                //    UserName = "admincentral@mail.com",
                //    LicenceCode = "1111111111"
                //};
                //if (!AddUserAdminAutomatic(centralOrganization.Id, AuthenticationType.AdminCentral, RoleType.AdminCentral, baseRegisterProgramModel))
                //    throw new Exception("");

                //ثبت رکورد واحد سازمان
                var branchProvince = new BranchProvince
                {
                    CentralOrganizationId = centralOrganization.Id,
                    Name = "واحد مرکز استان"
                };
                _branchProvinceRepository.Add(branchProvince);

                //baseRegisterProgramModel.RoleType = RoleType.AdminBranch;
                //baseRegisterProgramModel.Name = "مدیر واحد استان";
                //baseRegisterProgramModel.NationalCode = "2222222222";
                //baseRegisterProgramModel.Password = "adminbranch";
                //baseRegisterProgramModel.OrganName = "واحد مرکز استان";
                //baseRegisterProgramModel.UserName = "adminbranch@mail.com";
                //baseRegisterProgramModel.LicenceCode = "2222222222";

                //if(!AddUserAdminAutomatic(branchProvince.Id, AuthenticationType.AdminBranch, RoleType.AdminBranch, baseRegisterProgramModel))
                //    throw new Exception("");

                //ثبت رکورد دانشگاه
                var university = new University
                {
                    BranchProvinceId = branchProvince.Id,
                    Name = modelRegisterProgram.OrganName,
                    Address = modelRegisterProgram.Address,
                    Code = modelRegisterProgram.Code,
                    Phone = modelRegisterProgram.Phone
                };
                _universityRepository.Add(university);
                var setting = new Settings
                {
                    UniversityId = university.Id,
                    CommissionNumberRepetitions = 1,
                    CouncilNumberRepetitions = 1,
                    Ordinal = Ordinal.Ordinal
                };
                _settingsRepository.Add(setting);
                //_universityRepository.Add(university);

                if (!AddUserAdminAutomatic(university.Id, AuthenticationType.AdminUniversity, RoleType.AdminUniversity, modelRegisterProgram))
                    throw new Exception("");

                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return false;
            }
        }

        /// <summary>
        /// این متد رکورد مربوط به کاربر مدیر را در سطوح مختلف 
        /// به همراه احراز هویت ، نقش ، شخص ، پروفایل و انتساب نقش به یوزر وارد می کند
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="authenticationType"></param>
        /// <param name="roleType"></param>
        /// <param name="baseRegisterProgramModel"></param>
        /// <returns></returns>
        public virtual bool AddUserAdminAutomatic(long levelId, AuthenticationType authenticationType,
            RoleType roleType, BaseRegisterProgramModel baseRegisterProgramModel)
        {
            try
            {
                var hasher = new PasswordHasher();

                //ثبت رکورد احراز هویت برای مدیر دانشگاه
                Authentication authentication = new Authentication(levelId, authenticationType,
                    baseRegisterProgramModel.LicenceCode);
                _authenticationRepository.Add(authentication);

                //ثبت رکورد نقش مدیر دانشگاه
                Role role = new Role(levelId, roleType);
                _roleRepository.Add(role);

                //ایجاد کاربر برای مدیر دانشگاه
                User user = new User
                {
                    AuthenticationId = authentication.Id,
                    Email = baseRegisterProgramModel.UserName,
                    UserName = baseRegisterProgramModel.UserName,
                    PasswordHash = hasher.HashPassword(baseRegisterProgramModel.Password),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true
                };

                //ایجاد پروفایل
                Profile profile = new Profile
                {
                    Name = baseRegisterProgramModel.Name,
                    Family = baseRegisterProgramModel.Family,
                    NationalCode = baseRegisterProgramModel.NationalCode,
                    Mobile = baseRegisterProgramModel.Mobile
                };

                // ثبت رکورد جدول شخص و کاربر برای مدیر دانشگاه
                Person person = new Person
                {
                    User = user,
                    Profile = profile
                };
                if (authenticationType == AuthenticationType.AdminCentral)
                    person.CentralOrganizationId = levelId;
                else if (authenticationType == AuthenticationType.AdminBranch)
                    person.BranchProvinceId = levelId;
                else
                    person.UniversityId = levelId;

                _personRepository.Add(person);

                //انتساب نقش مدیریت به کاربر دانشگاه 
                UserRole userRole = new UserRole
                {
                    RoleId = role.Id,
                    UserId = person.Id
                };
                _userRoleRepository.Add(userRole);

                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// رکورد شخص و کاربر و پروفایل و دانشجو یا پرسنل ذخیره شده و همچنین سطح مربوط به تعریف شخص هم مشخص شده است
        /// </summary>
        /// <param name="identificationCode"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="nationalCode"></param>
        /// <param name="mobile"></param>
        /// <param name="name"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public virtual async Task<Tuple<bool, string, string, User>> AddUser(string identificationCode, string email, string password,
            string name, string family, string nationalCode, string mobile)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //_unitOfWork.BeginTransaction();
                    //var hasher = new PasswordHasher();

                    //واکشی احراز هویت مربوط به کد شناسایی
                    var authentication =
                        _authenticationRepository.Where(a => a.IdentityCode == identificationCode)
                            .Include(i => i.Users).FirstOrDefault();
                    if (authentication == null)
                        return new Tuple<bool, string, string, User>(false, "اطلاعات پایه کاربری وجود ندارد!", "", null);
                    if (authentication.Users.Any())
                        return new Tuple<bool, string, string, User>(false, "اطلاعات پایه کاربری قبلا استفاده شده است!", "", null);

                    //ایجاد پروفایل
                    var profile = new Profile
                    {
                        Name = name,
                        Family = family,
                        NationalCode = nationalCode,
                        Mobile = mobile
                    };

                    ////رکورد مربوط به دانشجو یا پرسنل
                    //switch (authentication.AuthenticationType)
                    //{
                    //    case AuthenticationType.Personel:
                    //        Personel personel = new Personel();
                    //        person.Personel = personel;
                    //        break;
                    //    case AuthenticationType.Student:
                    //        Student student = new Student();
                    //        person.Student = student;
                    //        break;
                    //}

                    //انتساب سطح تعریف شده مربوط به جدول احراز هویت به جدول شخص و ایجاد شخص
                    var person = new Person { Profile = profile };
                    switch (authentication.AuthenticationType)
                    {
                        case AuthenticationType.AdminCentral:
                            person.CentralOrganizationId = authentication.CentralOrganizationId;
                            break;
                        case AuthenticationType.AdminBranch:
                            person.BranchProvinceId = authentication.BranchProvinceId;
                            break;
                        default:
                            person.UniversityId = authentication.UniversityId;
                            break;
                    }
                    _personRepository.Add(person);


                    // ویرایش احراز هوییت بخش پر کردن پروفایل
                    var objectauthentication = _authenticationRepository.Find(p => p.IdentityCode == identificationCode);
                    objectauthentication.ExistinProfile = true;

                    // _authenticationRepository.Update(authentication);

                    _unitOfWork.SaveChanges();
                    //ایجاد کاربر
                    var user = new User
                    {
                        Id = person.Id,
                        AuthenticationId = authentication.Id,
                        Email = email,
                        UserName = email
                       //PasswordHash = hasher.HashPassword(registerUser.Password)
                    };
                    //PasswordHasher hashe = new PasswordHasher();
                  
                    var result = await _userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                        return new Tuple<bool, string, string, User>(false, "خطا در ثبت نام کاربر!", "", null);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    
                    //_unitOfWork.CommitTransaction();
                    scope.Complete();
                    return new Tuple<bool, string, string, User>(true, "", code, user);
                    //_unitOfWork.Rollback();
                }
            }
            catch (Exception ex)
            {
                //_unitOfWork.Rollback();
                return new Tuple<bool, string, string, User>(false, "خطا در ثبت نام کاربر!", "", null);
            }
        }

        /// <summary>
        /// این متد پس از احراز هویت و ایجاد رکورد لاگین ، رکورد کاربر مورد نظر را بر میگرداند 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<Tuple<bool, User>> Login(string email, string password)
        {
            try
            {
                //تایید کاربری
                var user = await _userManager.FindAsync(email, password);
                if (user == null)
                {
                    return new Tuple<bool, User>(false, null);
                }
                //تایید احراز هویت
                var authentication = _authenticationRepository.Where(a => a.Id == user.AuthenticationId).FirstOrDefault();
                if (authentication == null)
                {
                    return new Tuple<bool, User>(false, user);
                }
                //ایجاد رکورد مربوط به جدول لاگین کاربر
                _userLoginRepository.AddOrUpdate(new UserLogin { UserId = user.Id, LoginProvider = "111", ProviderKey = "222" });
                _unitOfWork.SaveChanges();
                return new Tuple<bool, User>(true, user);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, User>(false, null);
            }
        }

        /// <summary>
        /// claim کاربر را مقداردهی می کند
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public async Task<bool> SignInAsync(User user, bool isPersistent)
        {
            try
            {
                string levelId = "", levelProgram = "", organName = "", isAdmin = "false", isPersonel = "false";

                var userProfile = _userRepository.Where(u => u.Id == user.Id).Include(i => i.Authentication)
                    .Include(i => i.Authentication.CentralOrganization).Include(i => i.Authentication.BranchProvince)
                    .Include(i => i.Authentication.University).Include(i => i.Person).Include(i => i.Person.Profile)
                    .Include(i => i.Person.Personel).Include(i => i.Roles.Select(s => s.Role)).FirstOrDefault();

                if (userProfile != null)
                {
                    if (userProfile.Authentication.CentralOrganizationId > 0)
                    {
                        levelId = userProfile.Authentication.BranchProvinceId.ToString();
                        levelProgram = LevelProgram.CentralOrganization.ToString();
                        organName = userProfile.Authentication.CentralOrganization.Name;
                    }
                    else if (userProfile.Authentication.BranchProvinceId > 0)
                    {
                        levelId = userProfile.Authentication.BranchProvinceId.ToString();
                        levelProgram = LevelProgram.BranchProvince.ToString();
                        organName = userProfile.Authentication.BranchProvince.Name;
                    }
                    else
                    {
                        levelId = userProfile.Authentication.UniversityId.ToString();
                        levelProgram = LevelProgram.University.ToString();
                        organName = userProfile.Authentication.University.Name;
                    }
                    var aa = userProfile.Roles.Any(a => a.Role.RoleType == RoleType.AdminUniversity);
                    isAdmin = userProfile.Roles.Any(a => a.Role.RoleType == RoleType.AdminUniversity).ToString();
                    isPersonel = (userProfile.Person.Personel != null).ToString();
                }
                var signer = _signerRepository.Where(s => s.Post.PostPersons.Any(a => a.PersonId == user.Id) || s.Post.UserPosts.Any(a => a.UserId == user.Id)).Select(s => s.RowNumber).DistinctBy(d=>d).ToArray();
                var listSigner = string.Join("#", signer);
                var logo = _universityRepository.All().Select(s => s.Logo).FirstOrDefault() ?? " ";

                _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                if (identity == null)
                    return false;
                if (userProfile?.Person.Profile != null)
                {
                    identity.AddClaim(new Claim("FullName", userProfile.Person.Profile.FullName));
                    identity.AddClaim(new Claim("FirstName", userProfile.Person.Profile.Name));
                    identity.AddClaim(new Claim("LastName", userProfile.Person.Profile.Family));
                    identity.AddClaim(new Claim("AuthenType", userProfile.Authentication.AuthenticationType.ToString()));
                    identity.AddClaim(new Claim("LevelId", levelId));
                    identity.AddClaim(new Claim("LevelProgram", levelProgram));
                    identity.AddClaim(new Claim("OrganName", organName));
                    if (userProfile.Person.Profile.Avatar != null)
                        identity.AddClaim(new Claim("Avatar", userProfile.Person.Profile.Avatar));
                }
                identity.AddClaim(new Claim("ListSigner", listSigner));
                identity.AddClaim(new Claim("Logo", logo));
                identity.AddClaim(new Claim("IsAdmin", isAdmin));
                identity.AddClaim(new Claim("IsPersonel", isPersonel));
                _authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// این متد رکورد لاگین مربوط به کاربر را حذف میکند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<bool> Logout(long userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;
                //حذف رکورد مربوط به جدول لاگین کاربر
                var userLogin = _userLoginRepository.Find(ul => ul.UserId == user.Id);
                if (userLogin != null)
                    _userLoginRepository.Delete(userLogin);
                _unitOfWork.SaveChanges();
                _authenticationManager.SignOut();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            //_authenticationManager.SignOut();
        }

        /// <summary>
        /// ریست کردن پسورد و بازگرداندن آن در متغیر کد
        /// </summary>
        /// <param name="forgotPasswordModel"></param>
        /// <returns></returns>
        public virtual async Task<Tuple<bool, string>> ForgetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return new Tuple<bool, string>(false, string.Empty);
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                return new Tuple<bool, string>(true, code);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, string.Empty);
            }
        }

        /// <summary>
        /// مشخص می کند که آیا این برنامه برای اولین بار استفاده می شود یا نه!
        /// اگر اولین بار است که استفاده می شود ، در چه سطحی استفاده می شود
        /// </summary>
        /// <returns></returns>
        public virtual Tuple<bool, RoleType> LoadProgram()
        {
            try
            {
                int countCentral = _centralOrganizationRepository.Count;
                if (countCentral > 0)
                    return new Tuple<bool, RoleType>(true, LicenceProgramModel.RoleType);
                return new Tuple<bool, RoleType>(false, LicenceProgramModel.RoleType);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, RoleType>(false, LicenceProgramModel.RoleType);
            }
        }

        public virtual async Task<Tuple<bool, string>> ConfirmEmail(long userId, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new Tuple<bool, string>(false, "خطا در تایید کاربری");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return new Tuple<bool, string>(result.Succeeded, "");
        }

        /// <summary>
        /// بررسی می کند که آیا کاربر پروفایل خود را وارد کرده است یا خیر
        /// و همچنین سطح کاربر را نشان می دهد
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Tuple<bool, AuthenticationType> CheckProfile(long userId)
        {
            try
            {
                var user = _userRepository.Where(u => u.Id == userId).Include(i => i.Person).Include(i => i.Authentication).Include(i => i.Person.Profile).FirstOrDefault();
                if (user == null)
                    return new Tuple<bool, AuthenticationType>(false, AuthenticationType.None);
                return new Tuple<bool, AuthenticationType>(user.Person.Profile != null, user.Authentication.AuthenticationType);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, AuthenticationType>(false, AuthenticationType.None);
            }
        }
        public virtual async Task<Tuple<bool, User>> IsEmailExist(string email)
        {
            try
            {
                var u = await _userManager.FindByEmailAsync(email);
                return new Tuple<bool, User>(u != null, u);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, User>(false, null); ;
            }
        }
        public virtual async Task<Tuple<bool, User>> IsEmailConfirmed(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return new Tuple<bool, User>(false, null);
                if (await _userManager.IsEmailConfirmedAsync(user.Id))
                    return new Tuple<bool, User>(true, user);
                return new Tuple<bool, User>(false, null);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, User>(false, null);
            }
        }
        public virtual async Task<bool> HasValidIdentificationCode(string identificationCode)
        {
            try
            {
                var data = await _authenticationRepository.Where(c => c.IdentityCode == identificationCode && c.Users.Count == 0).FirstOrDefaultAsync();
                return data != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> HasValidNationalCode(string nationalCode)
        {
            try
            {
                var profile = await _profileRepository.Where(p => p.NationalCode == nationalCode).FirstOrDefaultAsync();
                return profile != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool AuthenticationProgram()
        {
            return true;
        }
        private Tuple<bool, Authentication> AuthenticationIdentificationCode(string identificationCode)
        {
            try
            {
                var authentication =
                           _authenticationRepository.Where(a => a.IdentityCode == identificationCode)
                               .FirstOrDefault();
                return new Tuple<bool, Authentication>(true, authentication);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, Authentication>(false, null);
            }
        }
        public Task SendEmailAsync(long userId, string subject, string body)
        {
            return _userManager.SendEmailAsync(userId, subject, body);
        }
        public Task<string> GeneratePasswordAsync(long userId)
        {
            return _userManager.GeneratePasswordResetTokenAsync(userId);
        }
        public Task<IdentityResult> ResetPasswordAsync(long userId, string token, string newPassword)
        {
            return _userManager.ResetPasswordAsync(userId, token, newPassword);
        }
    }
}
