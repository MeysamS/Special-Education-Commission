using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using System.Transactions;

namespace Comision.Service.ImplementationService
{
    public class RequestService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestRepository _requestRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMemberMasterRepository _memberMasterRepository;
        private readonly ISignerRepository _signerRepository;
        private readonly ICartableRepository _cartableRepository;
        private readonly IPostPersonRepository _postPersonRepository;
        private readonly IArchiveRepository _archiveRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly ICommissionRepository _commissionRepository;
        private readonly ICommissionSpecialEducationRepository _commissionSpecialEducationRepository;
        private readonly ICouncilRepository _councilRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IVoteRepository _voteRepository;

        public RequestService(IUnitOfWork unitOfWork, IRequestRepository requestRepository,
            IProfileRepository profileRepository, IStudentRepository studentRepository,
            IPersonRepository personRepository, IMemberMasterRepository memberMasterRepository,
            ISignerRepository signerRepository, ICartableRepository cartableRepository,
            IPostPersonRepository postPersonRepository, IArchiveRepository archiveRepository,
            IUserPostRepository userPostRepository, ICommissionRepository commissionRepository,
            ICouncilRepository councilRepository, IAttachmentRepository attachmentRepository,
            IVoteRepository voteRepository, ICommissionSpecialEducationRepository commissionSpecialEducationRepository)
        {
            _unitOfWork = unitOfWork;
            _requestRepository = requestRepository;
            _profileRepository = profileRepository;
            _studentRepository = studentRepository;
            _personRepository = personRepository;
            _memberMasterRepository = memberMasterRepository;
            _signerRepository = signerRepository;
            _cartableRepository = cartableRepository;
            _postPersonRepository = postPersonRepository;
            _archiveRepository = archiveRepository;
            _userPostRepository = userPostRepository;
            _commissionRepository = commissionRepository;
            _councilRepository = councilRepository;
            _attachmentRepository = attachmentRepository;
            _voteRepository = voteRepository;
            _commissionSpecialEducationRepository = commissionSpecialEducationRepository;
        }

        /// <summary>
        /// ذخیره موقت درخواست به همراه پروفایل و دانشجو و شخص
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public Tuple<bool, string> AddorUpdateRequest(RequestModel requestModel)
        {
            try
            {
                var student = _studentRepository.Where(s => s.StudentNumber == requestModel.StudentNumber).FirstOrDefault() ?? new Student();
                student.StudentNumber = requestModel.StudentNumber;
                student.FieldofStudyId = requestModel.FieldofStudyId;
                //student.Grade = requestModel.Grade;
                //student.MilitaryServiceStatus = requestModel.MilitaryServiceStatus;

                var profile = _profileRepository.Where(p => p.NationalCode == requestModel.NationalCode).FirstOrDefault() ?? new Profile
                {
                    Name = requestModel.Name,
                    Family = requestModel.Family,
                    NationalCode = requestModel.NationalCode,
                    //Gender = requestModel.Gender
                };

                var request = _requestRepository.Find(i => i.Id == requestModel.Id) ?? new Request();
                request.RequestType = requestModel.RequestType;
                request.NumberofRemainingUnits = requestModel.NumberofRemainingUnits;
                request.NumberofSpentUnits = requestModel.NumberofSpentUnits;
                request.RequestStatus = requestModel.RequestStatus;
                request.Description = requestModel.Description;

                long personId = 0;
                if (profile.PersonId == 0)
                {
                    Person person = new Person
                    {
                        Profile = profile,
                        Student = student
                    };
                    _personRepository.Add(person);
                    personId = person.Id;
                }
                else
                {
                    personId = profile.PersonId;
                    student.PersonId = profile.PersonId;
                    _studentRepository.AddOrUpdate(s => s.PersonId, student);
                }

                if (request.Id > 0)
                    _requestRepository.Update(request);
                else
                {
                    var member = _memberMasterRepository.Where(m => m.Active).FirstOrDefault();
                    request.PersonId = personId;
                    if (member != null) request.MemberMasterId = member.Id;
                    _requestRepository.Add(request);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت به درستی انجام شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت درخواست");
            }
        }

        /// <summary>
        /// این متد اطلاعات مربوط به کمیسیون و پروفایل و دانشجویی یک شخص خاص را می دهد
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public Tuple<bool, string, DetailRequestModel> DetailRequest(long requestId, AddressUrlFile addressUrlFile)
        {
            try
            {
                // جزیات درخواست
                var detailRequest = (from p in _requestRepository.Where(c => c.Id == requestId)
                                     select new DetailRequestModel
                                     {
                                         RequestId = p.Id,
                                         RequestType = p.RequestType,
                                         RequestStatus = p.RequestStatus,
                                         CommissionNumber = p.Commission != null ? (p.Commission.CommissionNumber) : p.Council.CouncilNumber,
                                         Date = p.Commission != null ? p.Commission.Date : p.Council.Date,
                                         Description = p.Commission != null ? (p.Commission.Description ?? "") : (p.Council.Description ?? ""),
                                         NumberofSpentUnits = p.NumberofSpentUnits,
                                         NumberofRemainingUnits = p.NumberofRemainingUnits,

                                         Name = p.Person.Profile.Name,
                                         Family = p.Person.Profile.Family,
                                         NationalCode = p.Person.Profile.NationalCode,
                                         Gender = p.Person.Profile.Gender,

                                         StudentNumber = p.Person.Student.StudentNumber,
                                         Grade = p.Person.Student.Grade,
                                         MilitaryServiceStatus = p.Person.Student.MilitaryServiceStatus,
                                         FieldofStudy = p.Person.Student.FieldofStudy.OrganizationStructureName.Name,
                                         FieldofStudyId = p.Person.Student.FieldofStudyId,
                                         EducationGroupId = p.Person.Student.FieldofStudy.EducationalGroupId,
                                         CollegeId = p.Person.Student.FieldofStudy.EducationalGroup.CollegeId,
                                         UniversityId = p.Person.Student.FieldofStudy.EducationalGroup.College.UniversityId,
                                         ProblemsCouncil = p.Council != null ? p.Council.ProblemText : " ",

                                         VoteText = p.Vote == null ? " " : p.Vote.VoteText
                                     }).FirstOrDefault();

                if (detailRequest == null)
                    return new Tuple<bool, string, DetailRequestModel>(false, "رکورد کمیسیون نظر یافت نشد!", null);
                ////var showMilitaryServiceStatus = detailRequest.MilitaryServiceStatus == MilitaryServiceStatus.Included
                ////                                && detailRequest.Gender == Gender.Male;


                bool showRefrenceTo =
                _cartableRepository.Contains(
                    i =>
                        i.RequestId == requestId &&
                        i.CartableValidation == CartableValidation.Valid && ((detailRequest.RequestType == RequestType.Comision && i.RowNumber == 5 && i.ReferTo) ||
                        (detailRequest.RequestType == RequestType.Council && i.RowNumber == 6 && i.ReferTo)));
                //_cartableRepository.Contains(
                //    i =>
                //        i.RequestId == requestId &&
                //        i.CartableValidation == CartableValidation.Valid && i.RowNumber == 6);


                var signers = (from signer in _signerRepository.Where(i => i.RequestType == detailRequest.RequestType &&
                               (showRefrenceTo || (detailRequest.RequestType == RequestType.Comision && i.RowNumber != 6) ||
                               (detailRequest.RequestType == RequestType.Council && i.RowNumber != 7)))
                               join itemsinger in _cartableRepository.Where(i => i.RequestId == requestId && i.CartableValidation == CartableValidation.Valid)
                                   on new { signer.PostId, signer.RowNumber } equals new { itemsinger.PostId, itemsinger.RowNumber } into signerCartablsSiners
                               from data in signerCartablsSiners.DefaultIfEmpty()
                               select new DetailRequestSignerModel
                               {
                                   PersonId = signer.Post.PostPersons.FirstOrDefault(pp => (signer.Post.PostType == PostType.University ?
                                        (pp.UniversityId == detailRequest.UniversityId) :
                                        (signer.Post.PostType == PostType.College ? (pp.CollegeId == detailRequest.CollegeId) :
                                        (signer.Post.PostType == PostType.EducationalGroup ? (pp.EducationalGroupId == detailRequest.EducationGroupId) :
                                        (pp.FieldofStudyId == detailRequest.FieldofStudyId))))) == null ? 440 :
                                        signer.Post.PostPersons.FirstOrDefault(pp => (signer.Post.PostType == PostType.University ?
                                        (pp.UniversityId == detailRequest.UniversityId) :
                                        (signer.Post.PostType == PostType.College ? (pp.CollegeId == detailRequest.CollegeId) :
                                        (signer.Post.PostType == PostType.EducationalGroup ? (pp.EducationalGroupId == detailRequest.EducationGroupId) :
                                        (pp.FieldofStudyId == detailRequest.FieldofStudyId))))).PersonId,
                                   PersonName =
                                        signer.Post.PostPersons.FirstOrDefault(pp => (signer.Post.PostType == PostType.University ?
                                            (pp.UniversityId == detailRequest.UniversityId) :
                                            (signer.Post.PostType == PostType.College ? (pp.CollegeId == detailRequest.CollegeId) :
                                            (signer.Post.PostType == PostType.EducationalGroup ? (pp.EducationalGroupId == detailRequest.EducationGroupId) :
                                        (pp.FieldofStudyId == detailRequest.FieldofStudyId))))) == null ? "پرسنل وجود ندارد" :
                                        signer.Post.PostPersons.Where(pp => (signer.Post.PostType == PostType.University ?
                                            (pp.UniversityId == detailRequest.UniversityId) :
                                            (signer.Post.PostType == PostType.College ? (pp.CollegeId == detailRequest.CollegeId) :
                                            (signer.Post.PostType == PostType.EducationalGroup ? (pp.EducationalGroupId == detailRequest.EducationGroupId) :
                                        (pp.FieldofStudyId == detailRequest.FieldofStudyId))))).Select(s => new { f = s.Person.Profile.Name + " " + s.Person.Profile.Family }).FirstOrDefault().f,
                                   PostId = signer.PostId,
                                   RowNumber = signer.RowNumber,
                                   Signature = data.Person.Personel.Signature != null ? addressUrlFile.Signature + data.Person.Personel.Signature : string.Empty,
                                   PostName = signer.Post.Name,
                                   Signed = data.Person.Personel != null
                               }).OrderBy(d => d.RowNumber).ToList();
                detailRequest.DetailRequestSignerModels = signers;

                return new Tuple<bool, string, DetailRequestModel>(true, "", detailRequest);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string, DetailRequestModel>(false, "خطا در انجام عملیات!", null);
            }
        }

        public Tuple<bool, string, List<Request>> GetListRequest(long userId)
        {
            try
            {
                // لیست ایدی رشته های که کاربر دارد به شرطی که سمت های ان جز امضا کننده گان باشد
                var posts =
                           _postPersonRepository.Where(p => p.PersonId == userId && p.Post.Signers.Any(s => s.RowNumber == 1))
                           .Select(f => f.FieldofStudyId).ToList();
                // اگر کاربر یک سری اختیارات از جدول سمت ها دارد ، سپس ایدی رشته های سمت های ان را می گیریم
                posts.AddRange(_userPostRepository.Where(p => p.UserId == userId && p.Post.Signers.Any(s => s.RowNumber == 1))
                           .Select(f => f.FieldofStudyId).ToList());

                // حال از جدول درخواست ها ان درخواست های را می گیریم که ایدی رشته ان در لیست سمت  باشدی
                var data = _requestRepository.Where(r => posts.Contains(r.Person.Student.FieldofStudyId) &&
                            (r.RequestStatus != RequestStatus.InFlow && r.RequestStatus != RequestStatus.Archive))
                            .Include(x => x.Person.Profile).Include(x => x.Person.Student.FieldofStudy.OrganizationStructureName)
                            .Include(x => x.Commission).Include(x => x.Council).Include(x => x.Vote).ToList();
                return new Tuple<bool, string, List<Request>>(true, "", data);
            }
            catch (Exception ex)
            { return new Tuple<bool, string, List<Request>>(false, "خطا در ثبت درخواست", null); }
        }

        public int GetRequestCount(long userId)
        {
            try
            {
                var posts =
                           _postPersonRepository.Where(p => p.PersonId == userId && p.Post.Signers.Any(s => s.RowNumber == 1))
                           .Select(x => x.FieldofStudyId).ToList();

                var requestCount = _requestRepository.Where(r => posts.Contains(r.Person.Student.FieldofStudyId) &&
                            (r.RequestStatus != RequestStatus.InFlow && r.RequestStatus != RequestStatus.Archive)).Count();
                return requestCount;
            }
            catch (Exception ex)
            { return -1; }
        }
        public IQueryable<Request> GetRequestByStudentCode(long studentNumber)
        {
            return _requestRepository.Where(x => x.Person.Student.StudentNumber == studentNumber)
                .Include(x => x.Commission)
                .Include(x => x.Council)
                .Include(x => x.Person)
                .Include(x => x.Person.Student)
                .Include(x => x.Person.Student.FieldofStudy)
                .Include(x => x.Person.Student.FieldofStudy.OrganizationStructureName)
                .Include(x => x.Person.Profile);
        }

        /// <summary>
        ///این متد یک در خواست را جستج می کند و در صورت یافتن برگشت می دهد
        /// در صورت عدم وجود در خواست مقدار نال را برگشت می دهد
        /// </summary>
        /// <param name="requestId">ایدی در خواست</param>
        /// <returns></returns>
        public Request Find(long requestId)
        {
            try
            {
                return _requestRepository.Find(i => i.Id == requestId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Tuple<bool, string> Archive(long requestId, long userId, DateTime dateArchive)
        {
            try
            {
                ////ابتدا در کارتابل ذخیره شود
                //bool Valid = InsertCartable(CartableStatus.Verdict, userId, requestId, postuserId, rowNumber, description);
                // رای صادر شود
                _archiveRepository.Add(new Archive { RequestId = requestId, PersonId = userId, Date = dateArchive });
                var upobject = _requestRepository.Find(qu => qu.Id == requestId);
                if (upobject != null)
                {
                    upobject.RequestStatus = RequestStatus.Archive;
                    _requestRepository.Update(upobject);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "بایگانی با موفقیت انجام شده است");

                //else
                //    return new Tuple<bool, string>(false, "درخواست مورد نظر در جریان نمی باشد و صدور رای امکان پذیر نیست");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "عملیات با مشکل مواجه شده است");
            }

        }
        public Tuple<bool, string> Delete(long requestId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {                    
                    _cartableRepository.Delete(c => c.RequestId == requestId);
                    _commissionRepository.Delete(c => c.RequestId == requestId);
                    _councilRepository.Delete(c => c.RequestId == requestId);
                    _attachmentRepository.Delete(a => a.RequestId == requestId);
                    _voteRepository.Delete(v => v.RequestId == requestId);
                    _archiveRepository.Delete(a => a.RequestId == requestId);
                    _commissionSpecialEducationRepository.Delete(c => c.CommissionId == requestId);
                    _requestRepository.Delete(r=>r.Id==requestId);
                    _unitOfWork.SaveChanges();
                    scope.Complete();
                    return new Tuple<bool, string>(true, "عملیات حذف با موفقیت انجام شده است");
                }
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "عملیات حذف با مشکل مواجه شده است");
            }
        }

        public virtual Tuple<bool, string, List<RequestStudentModel>> ListRequestStudent(long studentNumber)
        {
            try
            {
                var requestStudents = (from itemrequest in _requestRepository.Where(x => x.Person.Student.StudentNumber == studentNumber)
                              .Include(i => i.Person.Profile).Include(i => i.Person.Student).Include(i => i.Vote)
                              .Include(i => i.Vote).Include(i => i.Commission).Include(i => i.Council)
                                       select new RequestStudentModel
                                       {
                                           Vote = itemrequest.Vote,
                                           Name = itemrequest.Person.Profile.Name,
                                           Family = itemrequest.Person.Profile.Family,
                                           RequestType = itemrequest.RequestType,
                                           Description = itemrequest.Description,
                                           Id = itemrequest.Id,
                                           Gender = itemrequest.Person.Profile.Gender,
                                           StudentNumber = itemrequest.Person.Student.StudentNumber,
                                           RequestStatus = itemrequest.RequestStatus,
                                           NationalCode = itemrequest.Person.Profile.NationalCode,
                                           MilitaryServiceStatus = itemrequest.Person.Student.MilitaryServiceStatus,
                                           CommissionCouncilNumber = itemrequest.RequestType == RequestType.Comision ? itemrequest.Commission.CommissionNumber : itemrequest.Council.CouncilNumber,
                                           Grade = itemrequest.Person.Student.Grade,
                                           DateRequest = itemrequest.RequestType == RequestType.Comision ? itemrequest.Commission.Date : itemrequest.Council.Date,
                                           FieldofStudyName = itemrequest.Person.Student.FieldofStudy.OrganizationStructureName.Name

                                       }).ToList();

                if (requestStudents == null)
                    return new Tuple<bool, string, List<RequestStudentModel>>(false, "درخواست دانشجو  مورد نظر یافت نشد!", null);
                return new Tuple<bool, string, List<RequestStudentModel>>(false, "لیستی از درخواست های دانشجو یافت شده است!", requestStudents);

            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<RequestStudentModel>>(false, "خطا در انجام عملیات!", null);
            }
        }
    }
}
