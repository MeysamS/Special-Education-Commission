using System;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;

namespace Comision.Service.ImplementationService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AttachmentService(IAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork)
        {
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
        }
        public Tuple<bool, string> Add(AttachmentModel attachmentModel)
        {
            try
            {
                _attachmentRepository.Add(new Attachment { File = attachmentModel.File, Extention = attachmentModel.Extention, RequestId = attachmentModel.RequestId, Size = attachmentModel.Size, Title = attachmentModel.Title });
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        public IQueryable<Attachment> GetAll(long requestId)
        {
            return _attachmentRepository.Where(x => x.RequestId == requestId);
        }

        public Tuple<bool, string> Delete(long id)
        {
            try
            {
                _attachmentRepository.Delete(x => x.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        public Attachment Find(long id)
        {
            return _attachmentRepository.Find(x => x.Id == id);
        }
    }
}
