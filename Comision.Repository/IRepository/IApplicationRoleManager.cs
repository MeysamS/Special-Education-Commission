using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comision.Model.Domain.UserDomain;
using Microsoft.AspNet.Identity;

namespace Comision.Repository.IRepository
{
    public interface IApplicationRoleManager : IDisposable
    {
        /// <summary>
        /// Used to validate roles before persisting changes
        /// </summary>
        IIdentityValidator<Role> RoleValidator { get; set; }

        /// <summary>
        /// Returns an IQueryable of roles if the store is an IQueryableRoleStore
        /// </summary>
        IQueryable<Role> Roles { get; }

        /// <summary>
        /// Create a role
        /// </summary>
        /// <param name="role"/>
        /// <returns/>
        Task<IdentityResult> CreateAsync(Role role);

        /// <summary>
        /// Update an existing role
        /// </summary>
        /// <param name="role"/>
        /// <returns/>
        Task<IdentityResult> UpdateAsync(Role role);

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="role"/>
        /// <returns/>
        Task<IdentityResult> DeleteAsync(Role role);

        /// <summary>
        /// Returns true if the role exists
        /// </summary>
        /// <param name="roleName"/>
        /// <returns/>
        Task<bool> RoleExistsAsync(string roleName);

        /// <summary>
        /// Find a role by id
        /// </summary>
        /// <param name="roleId"/>
        /// <returns/>
        Task<Role> FindByIdAsync(long roleId);
        Role FindRoleById(long roleId);
        /// <summary>
        /// Find a role by name
        /// </summary>
        /// <param name="roleName"/>
        /// <returns/>
        Task<Role> FindByNameAsync(string roleName);


        // Our new custom methods

        Role FindRoleByName(string roleName);
        IdentityResult CreateRole(Role role);
        IList<UserRole> GetUsersInRole(string roleName);
        IList<User> GetApplicationUsersInRole(string roleName);
        IList<Role> FindUserRoles(long userId);
        string[] GetRolesForUser(long userId);
        bool IsUserInRole(long userId, string roleName);
        Task<List<Role>> GetAllRolesAsync();

        IQueryable<Role> GetAllRolesAsQueryable();
        IQueryable<UserRole> GetAllUserRole();
        bool AddUserRole(long userId, long roleId);
    }
}