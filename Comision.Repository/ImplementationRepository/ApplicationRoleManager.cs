using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;
using Microsoft.AspNet.Identity;

namespace Comision.Repository.ImplementationRepository
{
    public class ApplicationRoleManager : RoleManager<Role, long>, IApplicationRoleManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleStore<Role, long> _roleStore;
        private readonly IDbSet<UserRole> _userRoles;
        private readonly IDbSet<User> _users;
        public ApplicationRoleManager(IUnitOfWork uow, IRoleStore<Role, long> roleStore)
            : base(roleStore)
        {
            _uow = uow;
            _roleStore = roleStore;
            _users = _uow.Set<User>();
            _userRoles = _uow.Set<UserRole>();
        }
        public Role FindRoleById(long roleId)
        {
            return this.FindById(roleId); // RoleManagerExtensions
        }

        public Role FindRoleByName(string roleName)
        {
            return this.FindByName(roleName); // RoleManagerExtensions
        }

        public IdentityResult CreateRole(Role role)
        {
            return this.Create(role); // RoleManagerExtensions
        }

        public IList<UserRole> GetUsersInRole(string roleName)
        {
            return this.Roles.Where(role => role.Name == roleName)
                             .SelectMany(role => role.Users)
                             .ToList();
            // = this.FindByName(roleName).Users
        }

        public IList<User> GetApplicationUsersInRole(string roleName)
        {
            var roleUserIdsQuery = from role in this.Roles
                                   where role.Name == roleName
                                   from user in role.Users
                                   select user.UserId;
            return _users.Where(applicationUser => roleUserIdsQuery.Contains(applicationUser.Id))
                         .ToList();
        }

        public IList<Role> FindUserRoles(long userId)
        {
            var userRolesQuery = from role in this.Roles
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToList();
        }

        public string[] GetRolesForUser(long userId)
        {
            var roles = FindUserRoles(userId);
            if (roles == null || !roles.Any())
            {
                return new string[] { };
            }

            return roles.Select(x => x.Name).ToArray();
        }

        public bool IsUserInRole(long userId, string roleName)
        {
            var userRolesQuery = from role in this.Roles
                                 where role.Name == roleName
                                 from user in role.Users
                                 where user.UserId == userId
                                 select role;
            var userRole = userRolesQuery.FirstOrDefault();
            return userRole != null;
        }

        public Task<List<Role>> GetAllRolesAsync()
        {
            return this.Roles.ToListAsync();
        }

        public IQueryable<Role> GetAllRolesAsQueryable()
        {
            return this.Roles.AsQueryable();
        }
        public IQueryable<UserRole> GetAllUserRole()
        {
            return _userRoles.AsQueryable();
        }
        public bool AddUserRole(long userId, long roleId)
        {
            try
            {
                UserRole userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };
                _userRoles.Add(userRole);
                _uow.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}