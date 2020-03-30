using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class AttachmentRepository:MainRepository<Attachment>,IAttachmentRepository
    {
        readonly IDbSet<Attachment> _attachments;
        IUnitOfWork _unitOfWork;
        public AttachmentRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _attachments = _unitOfWork.Set<Attachment>();
        }
    }
}
