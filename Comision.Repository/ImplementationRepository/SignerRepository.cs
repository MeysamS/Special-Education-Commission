using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class SignerRepository : MainRepository<Signer>, ISignerRepository
    {
        readonly IDbSet<Signer> _signer;
        IUnitOfWork _unitOfWork;
        public SignerRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _signer = _unitOfWork.Set<Signer>();
        }
    }
}
