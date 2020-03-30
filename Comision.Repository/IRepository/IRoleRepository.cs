using System;
using Comision.Model.Domain.UserDomain;

namespace Comision.Repository.IRepository
{
    public interface IRoleRepository : IDisposable, IMainRepository<Role>
    {
    }
}