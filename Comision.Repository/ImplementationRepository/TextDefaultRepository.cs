using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class TextDefaultRepository : MainRepository<TextDefault>, ITextDefaultRepository
    {

        readonly IDbSet<TextDefault> _textDefault;
        IUnitOfWork _unitOfWork;
        public TextDefaultRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _textDefault = _unitOfWork.Set<TextDefault>();
        }
    }
}
