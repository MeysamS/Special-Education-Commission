using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface IPersonManagementService
    {
        Tuple<bool, string> AddOrUpdatePersonelProfile(PersonelProfileModel personelProfileModel, long universityId);
        Tuple<bool, string,Profile> AddOrUpdateProfile(ProfileModel modelProfile, long universityId);
        Tuple<bool, string> UpdateProfile(Profile model);
        Tuple<bool, string,Personel> AddOrUpdatePersonel(PersonelModel personelModel, long universityId);
        Tuple<bool, string> UpdatePersonel(Personel model);
        Tuple<bool, string,Student> AddOrUpdateStudent(StudentModel studentModel, long universityId);
        Tuple<bool, string> UpdateStudent(Student model);
        Tuple<bool, string> AddOrUpdateStudentProfile(StudentProfileModel studentProfileModel, long universityId);
        Tuple<bool, string,User> AddOrUpdateUser(UserModel userModel, long universityId);
        Tuple<bool, string> UpdateUser(User model);
        Tuple<bool, string> AddOrUpdatePassword(PasswordModel passwordModel);
        Tuple<bool, string> AddOrUpdateProfileFullInfo(ProfileFullInfoModel profileFullInfoModel, long universityId);
        Tuple<bool, string> AddOrUpdateAssignmentPost(PostPersonModel postPersonModel);
        Tuple<bool, string> AddOrUpdateAssignmentPostUser(PostPersonModel postPersonModel);
        Tuple<bool, string> AddOrUpdateAssignmentRole(UserRoleModel userRoleModel);
        Tuple<bool, string, string> DeletePersonel(long personId);
        Tuple<bool, string> DeleteStudent(long personId);
        Tuple<bool, string> DeleteAssignmentPost(long postId, long personId, PostType postType, long levelId);
        Tuple<bool, string> DeleteAssignmentPostUser(long postId, long personId, PostType postType, long levelId);
        Tuple<bool, string> DeleteAssignmentRole(long userId, long roleId);
    
        List<Person> GetUsers(Expression<Func<Person, bool>> orderByProperty,
            AuthenticationType userLoginAuthenticationType, UserType userType, long levelId, bool isAscendingOrder, out int pageCount,
            int pageNum = 1, int pageSize = 20);

        Person FindPerson(long personId);
        Profile FindProfile(long profileId);
        Personel FindPersonel(long personelId);
        Student FindStudent(long studentId);

        Tuple<bool, string, PersonelProfileModel> GetPersonelProfile(long personId);
        Tuple<bool, string, ProfileFullInfoModel> GetProfileFullInfo(long personId);
        Tuple<bool, string, List<PersonelProfileModel>> GetPersonelProfiles(LevelProgram levelProgram, long leveLId);
        Tuple<bool, string, List<PostPersonModel>> GetPostofPersonel(long personId);
        Tuple<bool, string, List<PostPersonModel>> GetPostofUser(long personId);
        Tuple<bool, string, List<PostPerson>> GetPostSignersofPersonel(long personId);
        Tuple<bool, string, List<UserRoleModel>> GetUserRoles(long userId);
        Tuple<bool, string, List<UserProfileModel>> GetUsers(LevelProgram levelProgram,
            long leveLId);
        Tuple<bool, string, StudentProfileModel> GetStudentProfile(long studentNumber, long userId);
        Tuple<bool, string, RequestModel> IsStudentExist(long studentNumber, long userId);
        long GetPersonIdbyStudentNumber(long studentNumber);
    }
}
