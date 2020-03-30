using System;
using System.Threading.Tasks;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Service.Model;
using Microsoft.AspNet.Identity;

namespace Comision.Service.IService
{
    public interface IAuthenticationManagementService
    {
        bool RegisterProgramCentralOrganization(RegisterProgramCentralOrganizationModel modelRegisterProgram);
        bool RegisterProgramBranchProvince(RegisterProgramBranchProvinceModel modelRegisterProgram);
        bool RegisterProgramUniversity(RegisterProgramUniversityModel modelRegisterProgram);
        bool AddUserAdminAutomatic(long levelId, AuthenticationType authenticationType,
            RoleType roleType, BaseRegisterProgramModel baseRegisterProgramModel);
        Tuple<bool, RoleType> LoadProgram();

        Task<Tuple<bool, string, string, User>> AddUser(string identificationCode, string email, string password, string name,
            string family, string nationalCode, string mobile);
        Task<Tuple<bool, User>> Login(string email, string password);
        Task<bool> SignInAsync(User user, bool isPersistent);
        Task<bool> Logout(long userId);
        Task<Tuple<bool, string>> ForgetPassword(string email);
        Tuple<bool, AuthenticationType> CheckProfile(long userId);
        Task<Tuple<bool, string>> ConfirmEmail(long userId, string code);
        Task<Tuple<bool,User>> IsEmailExist(string email);
        Task<Tuple<bool, User>> IsEmailConfirmed(string email);
        Task<bool> HasValidIdentificationCode(string identificationCode);
        Task<bool> HasValidNationalCode(string nationalCode);
        Task<IdentityResult> ResetPasswordAsync(long userId, string token, string newPassword);
        Task SendEmailAsync(long userId, string subject, string body);
        Task<string> GeneratePasswordAsync(long userId);
    }
}
