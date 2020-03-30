using System;
using System.Linq;
using Comision.Model.Domain;
using Comision.Service.Enum;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface IAttachmentService
    {
        Tuple<bool, string> Add(AttachmentModel attachmentModel);

        IQueryable<Attachment> GetAll(long requestId);

        Tuple<bool, string> Delete(long id);

        Attachment Find(long id);
    }
}
