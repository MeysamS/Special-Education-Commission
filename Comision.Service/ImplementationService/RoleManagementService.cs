using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;

namespace Comision.Service.ImplementationService
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly IPostPersonRepository _postPersonRepository;
        private readonly ISignerRepository _signerRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IAccessControlService _accessControlService;

        public RoleManagementService(IUnitOfWork unitOfWork, IPostRepository postRepository,
             IPostPersonRepository postPersonRepository, ISignerRepository signerRepository,
             IAuthenticationRepository authenticationRepository, IRoleRepository roleRepository,
             IUserRepository userRepository, IUserRoleRepository userRoleRepository,
             IAccessControlService accessControlService)
        {
            _unitOfWork = unitOfWork;
            _postRepository = postRepository;
            _postPersonRepository = postPersonRepository;
            _signerRepository = signerRepository;
            _authenticationRepository = authenticationRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _accessControlService = accessControlService;
        }

        public Tuple<bool, string> AddOrUpdateAuthentication(AuthenticationModel authenticationModel, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                {
                    var authentication = _authenticationRepository.Find(x => x.IdentityCode == authenticationModel.IdentityCode);
                    if (authentication != null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : کد شناسایی مورد نظر تکراری می باشد");
                    }
                    Authentication newAuthentication = new Authentication
                    {
                        AuthenticationType = authenticationModel.AuthenticationType,
                        IdentityCode = authenticationModel.IdentityCode,
                        CentralOrganizationId = authenticationModel.CentralOrganizationId,
                        BranchProvinceId = authenticationModel.BranchProvinceId,
                        UniversityId = authenticationModel.UniversityId
                    };
                    _authenticationRepository.Add(newAuthentication);
                }
                else
                {
                    if (_authenticationRepository.Contains(x => x.Id != authenticationModel.Id && x.IdentityCode == authenticationModel.IdentityCode))
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : کد شناسایی مورد نظر تکراری می باشد");
                    }
                    var authentication = _authenticationRepository.Find(x => x.Id == authenticationModel.Id);
                    if (authentication == null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");
                    }
                    authentication.IdentityCode = authenticationModel.IdentityCode;
                    authentication.AuthenticationType = authenticationModel.AuthenticationType;
                    //authentication.CentralOrganizationId = authenticationModel.CentralOrganizationId;
                    //authentication.BranchProvinceId = authenticationModel.BranchProvinceId;
                    //authentication.UniversityId = authenticationModel.UniversityId;
                    _authenticationRepository.Update(authentication);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> AddListAuthentication(List<AuthenticationModel> listAuthenticationModel, StateOperation stateOperation)
        {
            try
            {
                var listAuthen = listAuthenticationModel.Select(item => new Authentication
                {
                    IdentityCode = item.IdentityCode,
                    AuthenticationType = item.AuthenticationType,
                    CentralOrganizationId = item.CentralOrganizationId,
                    BranchProvinceId = item.BranchProvinceId,
                    UniversityId = item.UniversityId
                }).ToList();
                _authenticationRepository.Add(listAuthen);

                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> AddOrUpdateRole(RoleModel roleModel, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                {
                    var exist = _roleRepository.Find(x => x.Name == roleModel.Name || x.NameFa == roleModel.NameFa);
                    if (exist != null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام نقش وجود دارد");
                    }
                    var role = new Role
                    {
                        Name = roleModel.Name,
                        NameFa = roleModel.NameFa,
                        CentralOrganizationId = roleModel.CentralOrganizationId,
                        BranchProvinceId = roleModel.BranchProvinceId,
                        UniversityId = roleModel.UniversityId,
                        RoleType = roleModel.RoleType,
                        XmlRoleAccess = roleModel.XmlRoleAccess
                    };
                    _roleRepository.Add(role);
                }
                else
                {
                    if (_roleRepository.Contains(x => x.Name == roleModel.Name && x.Id != roleModel.Id))
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام نقش(لاتین) وجود دارد");
                    }
                    var role = _roleRepository.Find(x => x.Id == roleModel.Id);
                    if (role == null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");
                    }
                    role.NameFa = roleModel.NameFa;
                    role.Name = roleModel.Name;
                    role.XmlRoleAccess = roleModel.XmlRoleAccess;
                    _roleRepository.Update(role);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> AddOrUpdatePost(PostModel viewModelPost, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                {
                    var exist = _postRepository.Find(x => x.Name == viewModelPost.Name);
                    if (exist != null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام سمت وجود دارد");
                    }
                    Post post = new Post
                    {
                        Name = viewModelPost.Name,
                        PostType = viewModelPost.PostType
                    };
                    _postRepository.Add(post);
                }
                else
                {
                    if (_postRepository.Contains(x => x.Name == viewModelPost.Name && x.Id != viewModelPost.Id))
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام سمت وجود دارد");
                    }
                    var post = _postRepository.Find(x => x.Id == viewModelPost.Id);
                    if (post == null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");
                    }
                    post.Name = viewModelPost.Name;
                    post.PostType = viewModelPost.PostType;

                    _postRepository.Update(post);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت علیات ");
            }
        }
        public IQueryable<Authentication> GetAllAuthentication(AuthenticationType userLoginAuthenticationType, long levelId)
        {
            if (userLoginAuthenticationType == AuthenticationType.AdminCentral)
                return _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminCentral && a.CentralOrganizationId == levelId);
            if (userLoginAuthenticationType == AuthenticationType.AdminBranch)
                return _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminBranch && a.BranchProvinceId == levelId);
            return _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminUniversity && a.UniversityId == levelId);
        }
        public IQueryable<Authentication> GetAuthenticationPaged(Expression<Func<Authentication, bool>> orderByProperty,
            AuthenticationType userLoginAuthenticationType, long levelId, bool isAscendingOrder, out int pageCount, int pageNum = 1, int pageSize = 20)
        {
            IQueryable<Authentication> authentications;
            if (userLoginAuthenticationType == AuthenticationType.AdminCentral)
                authentications = _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminCentral && a.CentralOrganizationId == levelId);
            else if (userLoginAuthenticationType == AuthenticationType.AdminBranch)
                authentications = _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminBranch && a.BranchProvinceId == levelId);
            else
                authentications = _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminUniversity && a.UniversityId == levelId);

            authentications = authentications.Include(i => i.Users);
            return _authenticationRepository.PagedResult(authentications, orderByProperty, isAscendingOrder, out pageCount, pageNum, pageSize);
        }
        public IQueryable<Authentication> GetAuthenticationByConditions(Expression<Func<Authentication, bool>> predicate, AuthenticationType userLoginAuthenticationType, long levelId)
        {
            IQueryable<Authentication> authentications;
            if (userLoginAuthenticationType == AuthenticationType.AdminCentral)
                authentications = _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminCentral && a.CentralOrganizationId == levelId);
            else if (userLoginAuthenticationType == AuthenticationType.AdminBranch)
                authentications = _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminBranch && a.BranchProvinceId == levelId);
            else
                authentications = _authenticationRepository.Where(a => a.AuthenticationType != AuthenticationType.AdminUniversity && a.UniversityId == levelId);

            return authentications.Where(predicate);
        }
        public IQueryable<Role> GetAllRole(AuthenticationType userLoginAuthenticationType, long levelId)
        {
            switch (userLoginAuthenticationType)
            {
                case AuthenticationType.AdminCentral:
                    return _roleRepository.Where(a => a.RoleType != RoleType.AdminCentral && a.CentralOrganizationId == levelId);
                case AuthenticationType.AdminBranch:
                    return _roleRepository.Where(a => a.RoleType != RoleType.AdminBranch && a.BranchProvinceId == levelId);
            }
            return _roleRepository.Where(a => a.RoleType != RoleType.AdminUniversity && a.UniversityId == levelId);
        }

        /// <summary>
        /// لیست اکشن ها و دسترسیهای یک نقش بر اساس قالب 
        /// لیست ارسال می شود
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Tuple<bool, List<ControllerModel>> GetAccessRoleXml(long roleId)
        {
            try
            {
                var role = _roleRepository.Find(sel => sel.Id == roleId);

                // تبدیل ایکس ام ل به لیست دسترسی ها

                //XmlUtility xmlutility=new XmlUtility();
                var lsit = _accessControlService.ConvertXmlToListControllers(role.XmlRoleAccess);
                return lsit != null ? new Tuple<bool, List<ControllerModel>>(true, lsit) :
                    new Tuple<bool, List<ControllerModel>>(false, null);
            }
            catch (Exception)
            {
                return new Tuple<bool, List<ControllerModel>>(false, null);
            }
        }
        public IQueryable<Role> GetRolePaged(Expression<Func<Role, bool>> orderByProperty, AuthenticationType userLoginAuthenticationType,
             long levelId, bool isAscendingOrder, out int pageCount, int pageNum = 1, int pageSize = 20)
        {
            IQueryable<Role> roles;
            if (userLoginAuthenticationType == AuthenticationType.AdminCentral)
                roles = _roleRepository.Where(a => a.RoleType != RoleType.AdminCentral && a.CentralOrganizationId == levelId);
            else if (userLoginAuthenticationType == AuthenticationType.AdminBranch)
                roles = _roleRepository.Where(a => a.RoleType != RoleType.AdminBranch && a.BranchProvinceId == levelId);
            else
                roles = _roleRepository.Where(a => a.RoleType != RoleType.AdminUniversity && a.UniversityId == levelId);

            return _roleRepository.PagedResult(roles, orderByProperty, isAscendingOrder, out pageCount, pageNum, pageSize);
        }
        public List<Post> GetAllPost()
        {
            try
            {
                return _postRepository.All().ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// لیست تمام پست های استفاده شده در جدول امضا کننده 
        /// </summary>
        /// <returns></returns>
        public List<Post> GetAllPostSigners()
        {
            try
            {
                var post = _signerRepository.All().Select(p => p.Post).Distinct().ToList();
                return post;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IQueryable<Post> GetPostPaged(Expression<Func<Post, bool>> orderByProperty,
             bool isAscendingOrder, out int pageCount, int pageNum = 1, int pageSize = 20)
        {
            var posts = _postRepository.All();
            return _postRepository.PagedResult(posts, orderByProperty, isAscendingOrder, out pageCount, pageNum, pageSize);
        }
        public long CountPosts()
        {
            try
            {
                return _postRepository.Count;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public Tuple<bool, string> DeleteAuthentication(long id)
        {
            try
            {
                if (_userRepository.Contains(x => x.AuthenticationId == id))
                {
                    return new Tuple<bool, string>(false, "رکورد مورد نظر استفاده شده است ، لذا قابل حذف نمی باشد");
                }
                _authenticationRepository.Delete(x => x.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف به درستی انجام شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> DeleteRole(long id)
        {
            try
            {
                if (_userRoleRepository.Contains(x => x.RoleId == id))
                {
                    return new Tuple<bool, string>(false, "رکورد مورد نظر استفاده شده است ، لذا قابل حذف نمی باشد!");
                }
                _roleRepository.Delete(x => x.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> DeletePost(long idPost)
        {
            try
            {
                if (_signerRepository.Contains(d => d.PostId == idPost))
                    return new Tuple<bool, string>(false, " سمت مورد نظر در لیست امظا کننده گان وجود دارد و قابل حذف نیست");
                if (_postPersonRepository.Contains(d => d.PostId == idPost))
                    return new Tuple<bool, string>(false, "سمت مورد نظر به برخی از اشخاص انتساب داده شده است  و قابل حذف نیست");
                _postRepository.Delete(p => p.Id == idPost);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, " حذف با موفقیت انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "عملیات حذف با خطا مواجه شده است");
            }
        }
    }
}
