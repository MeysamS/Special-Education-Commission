using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    /// <summary>
    /// سرویس مدیرست نقش و سمت ها
    /// </summary>
    public interface IRoleManagementService
    {

        Tuple<bool, string> AddOrUpdateAuthentication(AuthenticationModel authenticationModel,
            StateOperation stateOperation);
        Tuple<bool, string> AddListAuthentication(List<AuthenticationModel> listAuthenticationModel,
            StateOperation stateOperation);
        Tuple<bool, string> AddOrUpdateRole(RoleModel roleModel, StateOperation stateOperation);

        /// <summary>
        /// درج یا ویرایش
        /// </summary>
        /// <param name="viewModelPost"></param>
        /// <param name="stateOperation"></param>
        /// <returns>خروجی اول مقدار صحیح - خروجی دوم پیغام متد</returns>
        Tuple<bool, string> AddOrUpdatePost(PostModel viewModelPost, StateOperation stateOperation);
        IQueryable<Authentication> GetAllAuthentication(AuthenticationType userLoginAuthenticationType, long levelId);
        IQueryable<Authentication> GetAuthenticationPaged(Expression<Func<Authentication, bool>> orderByProperty, AuthenticationType userLoginAuthenticationType,
             long levelId, bool isAscendingOrder, out int pageCount, int pageNum = 1, int pageSize = 20);
        IQueryable<Authentication> GetAuthenticationByConditions(Expression<Func<Authentication, bool>> predicate, AuthenticationType userLoginAuthenticationType, long levelId);
        IQueryable<Role> GetAllRole(AuthenticationType userLoginAuthenticationType, long levelId);

        IQueryable<Role> GetRolePaged(Expression<Func<Role, bool>> orderByProperty,
            AuthenticationType userLoginAuthenticationType,
            long levelId, bool isAscendingOrder, out int pageCount, int pageNum = 1, int pageSize = 20);

        /// <summary>
        /// لیست از پست ها برا نمایش در گرید
        /// </summary>
        /// <returns>در صورت بروز خطا مقدار نال را بر میگرداند</returns>
        List<Post> GetAllPost();

        IQueryable<Post> GetPostPaged(Expression<Func<Post, bool>> orderByProperty,
            bool isAscendingOrder, out int pageCount, int pageNum = 1, int pageSize = 20);

        /// <summary>
        /// لیست تعداد پست ها
        /// </summary>
        /// <returns></returns>
        long CountPosts();
        Tuple<bool, string> DeleteAuthentication(long id);
        Tuple<bool, string> DeleteRole(long id);

        /// <summary>
        /// حذف پست
        /// </summary>
        /// <returns>در صورت حذف مقداردرست برگشت داده می شود</returns>
        Tuple<bool, string> DeletePost(long idPost);

        Tuple<bool, List<ControllerModel>> GetAccessRoleXml(long roleId);
        List<Post> GetAllPostSigners();
    }
}
