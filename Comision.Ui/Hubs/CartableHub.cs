using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace Comision.Ui.Hubs
{
    public class CartableHub : Hub
    {
        private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();
        private readonly IRequestService _requestService;
        //private readonly IUserService _userService;
        private readonly IPersonManagementService _personManagementService;
        private readonly IApplicationRoleManager _roleManager;
        private readonly ICartableService _cartableService;
        public CartableHub(IRequestService requestService, ICartableService cartableService,
            IPersonManagementService personManagementService, IApplicationRoleManager roleManager)
        {
            _requestService = requestService;
            _personManagementService = personManagementService;
            _roleManager = roleManager;
            _cartableService = cartableService;
        }
        public void Join()
        {
            var userName = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            var user = Users.GetOrAdd(userName, _ => new User
            {
                UserName = userName,
                ConnectionIds = new HashSet<string>()
            });
            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
            }
        }
        public void UnJoin()
        {
            var userName = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            User user;
            Users.TryGetValue(userName, out user);
            if (user == null) return;
            lock (user.ConnectionIds)
            {
                user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                if (user.ConnectionIds.Any()) return;
                User removeUser;
                Users.TryRemove(userName, out removeUser);
            }
        }
        public void AddNewCartableNotification()
        {
            var userId = Convert.ToInt64(Context.User.Identity.GetUserId());
            Clients.Others.sendNewCartable();
            Clients.Others.reloadRequest();
            Clients.All.getCartableCount(_cartableService.GetCartableCount(userId));
            Clients.All.getRequestCount(_requestService.GetRequestCount(userId));
            //Clients.Client(Context.ConnectionId).getCartableCount(_cartableService.GetAllCartablesAsQueryable().Count(x => x.Active == true && x.UserReciveId == uidRecive));
        }

        public class User
        {
            public string UserName { get; set; }
            public HashSet<string> ConnectionIds { get; set; }
        }
    }
}