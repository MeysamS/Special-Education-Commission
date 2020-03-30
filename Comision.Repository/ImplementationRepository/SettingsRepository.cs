using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{

    public class SettingsRepository : MainRepository<Settings>, ISettingsRepository
    {
        readonly IDbSet<Settings> _settings;
        IUnitOfWork _unitOfWork;
        public SettingsRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _settings = _unitOfWork.Set<Settings>();
        }
    }
}
