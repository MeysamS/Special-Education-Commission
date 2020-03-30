using System;
using System.Data.Entity;
using System.Threading;
using System.Web;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.ImplementationRepository;
using Comision.Repository.IRepository;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap;
using StructureMap.Web;

namespace Comision.IOC
{
    public static class SmObjectFactory
    {
       private static readonly Lazy<Container> ContainerBuilder =
            new Lazy<Container>(DefaultContainer,LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container { get { return ContainerBuilder.Value; } }
        
        public static Container DefaultContainer()
        {
            return new Container(cfg =>
            {
                cfg.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<ComisionDbContext>();
                cfg.For<ComisionDbContext>().HybridHttpOrThreadLocalScoped().Use<ComisionDbContext>();
                cfg.For<DbContext>().HybridHttpOrThreadLocalScoped().Use<ComisionDbContext>();

                cfg.For<IUserStore<User, long>>().HybridHttpOrThreadLocalScoped().Use<UserStore<User, Role, long, UserLogin,UserRole, UserClaim>>();
                cfg.For<IRoleStore<Role, long>>().HybridHttpOrThreadLocalScoped().Use<RoleStore<Role, long, UserRole>>();
                cfg.For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);
                cfg.For<IApplicationSignInManager>().HybridHttpOrThreadLocalScoped().Use<ApplicationSignInManager>();
                cfg.For<IApplicationUserManager>().HybridHttpOrThreadLocalScoped().Use<ApplicationUserManager>();

                cfg.For<IApplicationRoleManager>().HybridHttpOrThreadLocalScoped().Use<ApplicationRoleManager>();

               cfg.For<IArchiveRepository>().Use<ArchiveRepository>();
                cfg.For<IAuthenticationRepository>().Use<AuthenticationRepository>(); 
                cfg.For<IBranchProvinceRepository>().Use<BranchProvinceRepository>();
                cfg.For<ICentralOrganizationRepository>().Use<CentralOrganizationRepository>();
                cfg.For<ICollegeRepository>().Use<CollegeRepository>();
                cfg.For<ICommissionRepository>().Use<CommissionRepository>();
                cfg.For<ICommissionSpecialEducationRepository>().Use<CommissionSpecialEducationRepository>();
                cfg.For<ICouncilRepository>().Use<CouncilRepository>();
                cfg.For<IEducationalGroupRepository>().Use<EducationalGroupRepository>();
                cfg.For<IFieldofStudyRepository>().Use<FieldofStudyRepository>();
                cfg.For<IMemberDetailsRepository>().Use<MemberDetailsRepository>();
                cfg.For<IMemberMasterRepository>().Use<MemberMasterRepository>();
                cfg.For<IOrganizationStructureNameRepository>().Use<OrganizationStructureNameRepository>();
                cfg.For<IPermissionRepository>().Use<PermissionRepository>();
                cfg.For<IPersonelRepository>().Use<PersonelRepository>();
                cfg.For<IPersonRepository>().Use<PersonRepository>();
                cfg.For<IPostPersonRepository>().Use<PostPersonRepository>();
                cfg.For<IPostRepository>().Use<PostRepository>();
                cfg.For<IProfileRepository>().Use<ProfileRepository>();
                cfg.For<ICartableRepository>().Use<CartableRepository>();
                cfg.For<IRequestRepository>().Use<RequestRepository>();
                cfg.For<IRolePermissionRepository>().Use<RolePermissionRepository>();
                cfg.For<IRoleRepository>().Use<RoleRepository>();
                cfg.For<ISignerRepository>().Use<SignerRepository>();
                cfg.For<ISpecialEducationRepository>().Use<SpecialEducationRepository>();
                cfg.For<IStudentRepository>().Use<StudentRepository>();
                cfg.For<IUniversityRepository>().Use<UniversityRepository>();
                cfg.For<IUserClaimRepository>().Use<UserClaimRepository>();
                cfg.For<IUserLoginRepository>().Use<UserLoginRepository>();
                cfg.For<IUserRepository>().Use<UserRepository>();
                cfg.For<IUserRoleRepository>().Use<UserRoleRepository>();
                cfg.For<IVoteRepository>().Use<VoteRepository>();
                cfg.For<ITextDefaultRepository>().Use<TextDefaultRepository>();
                cfg.For<ISettingsRepository>().Use<SettingsRepository>();
                cfg.For<ISliderRepository>().Use<SliderRepository>();
                cfg.For<IAttachmentRepository>().Use<AttachmentRepository>();
                cfg.For<IUserPostRepository>().Use<UserPostRepository>();

                cfg.For<IAuthenticationManagementService>().Use<AuthenticationManagementService>();
                cfg.For<IPersonManagementService>().Use<PersonManagementService>();
                cfg.For<IFileManagementService>().Use<FileManagementService>();
                cfg.For<IRoleManagementService>().Use<RoleManagementService>();
                cfg.For<IStructureManageService>().Use<StructureManageService>();
                cfg.For<IBaseInfoComissionCouncilService>().Use<BaseInfoComissionCouncilService>();
                cfg.For<IMemberManagementService>().Use<MemberManagementService>();
                cfg.For<ICommissionService>().Use<CommissionService>();
                cfg.For<ICouncilService>().Use<CouncilService>();
                cfg.For<IRequestService>().Use<RequestService>();
                cfg.For<ICartableService>().Use<CartableService>();
                cfg.For<IAttachmentService>().Use<AttachmentService>();
                cfg.For<ITextDefaultService>().Use<TextDefaultService>();
                cfg.For<ISettingService>().Use<SettingService>();
                cfg.For<IReportsService>().Use<ReportsService>();
                cfg.For<ISliderService>().Use<SliderService>();
                cfg.For<IIdentityMessageService>().Use<SmsService>();
                cfg.For<IIdentityMessageService>().Use<EmailService>();
                cfg.For<IAccessControlService>().Use<AccessControlService>();
            });
        }

    }
}
