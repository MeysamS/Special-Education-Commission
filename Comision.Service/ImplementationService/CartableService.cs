using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Repository.ImplementationRepository;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;

namespace Comision.Service.ImplementationService
{
    public class CartableService : ICartableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostPersonRepository _postPersonRepository;
        private readonly IFieldofStudyRepository _fieldofStudyRepository;
        private readonly ICartableRepository _cartableRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly ISignerRepository _signerRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IPersonelRepository _personelRepository;
        private readonly ICommissionRepository _commissionRepository;
        private readonly ICouncilRepository _councilRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly IPostRepository _postRepository;
        private readonly IRequestService _requestService;
        private readonly ICommissionService _commissionService;
        private readonly ICouncilService _councilService;
        public CartableService(IUnitOfWork unitOfWork, IPostPersonRepository postPersonRepository,
            IFieldofStudyRepository fieldofStudyRepository, ICartableRepository cartableRepository,
            RequestRepository requestRepository, IVoteRepository voteRepository,
            ISignerRepository signerRepository, ISettingsRepository settingsRepository,
            IPersonelRepository personelRepository, ICommissionRepository commissionRepository,
            ICouncilRepository councilRepository, IRequestService requestService,
            IUserPostRepository userPostRepository, IPostRepository postRepository,
            ICommissionService commissionService, ICouncilService councilService)
        {
            _unitOfWork = unitOfWork;
            _postPersonRepository = postPersonRepository;
            _fieldofStudyRepository = fieldofStudyRepository;
            _cartableRepository = cartableRepository;
            _requestRepository = requestRepository;
            _signerRepository = signerRepository;
            _voteRepository = voteRepository;
            _settingsRepository = settingsRepository;
            _personelRepository = personelRepository;
            _commissionRepository = commissionRepository;
            _councilRepository = councilRepository;
            _requestService = requestService;
            _userPostRepository = userPostRepository;
            _postRepository = postRepository;
            _commissionService = commissionService;
            _councilService = councilService;
        }

        public Tuple<bool, string, List<CartableViewModel>, ArgumentException> GetListCartable(long userId)
        {
            try
            {
                var ordinal1 = _settingsRepository.All().Select(s => s.Ordinal).FirstOrDefault();
                var ordinal = (int)ordinal1; //برای مشخص کردن حالت ترتیبی یا غیر ترتیبی کارتابل
                var signers = _signerRepository.All().Include(i => i.Post).OrderBy(o => o.RowNumber); // لیست همه امضاکنندگان کمیسیون و شورا به ترتیب
                var lastCommissionSigner = signers.Where(w => w.RequestType == RequestType.Comision).Select(s => s.RowNumber).ToList().LastOrDefault();
                var lastCouncilSigner = signers.Where(w => w.RequestType == RequestType.Council).Select(s => s.RowNumber).ToList().LastOrDefault();

                var postsofPerson = _postPersonRepository.Where(p => p.PersonId == userId && p.Post.Signers.Any(s => s.RowNumber != 1))
                    .Include(i => i.Post).Include(i => i.Post.Signers).ToList(); // همه سمت ها به همراه رکوردهای امضاکننده کمیسیون و شورا مربوط به یک شخص
                postsofPerson.AddRange(_userPostRepository.Where(p => p.UserId == userId && p.Post.Signers.Any(s => s.RowNumber != 1))
                    .Include(i => i.Post).Include(i => i.Post.Signers).ToList()
                    .Select(s => new PostPerson
                    {
                        Post = s.Post,
                        BranchProvinceId = s.BranchProvinceId,
                        CentralOrganizationId = s.CentralOrganizationId,
                        CollegeId = s.CollegeId,
                        EducationalGroupId = s.EducationalGroupId,
                        PersonId = s.UserId,
                        PostId = s.PostId,
                        UniversityId = s.UniversityId
                        //Person = s.User.Person

                    }).ToList()); // همه سمت ها به همراه رکوردهای امضاکننده کمیسیون و شورا مربوط به یک شخص

                var postsofPersonCommission = postsofPerson.Where(p => p.Post.Signers.Any(s => s.RequestType == RequestType.Comision)).ToList(); // همه سمت ها به همراه رکوردهای امضاکننده کمیسیون مربوط به یک شخص
                var postsofPersonCouncil = postsofPerson.Where(p => p.Post.Signers.Any(s => s.RequestType == RequestType.Council)).ToList(); // همه سمت ها به همراه رکوردهای امضاکننده شورا مربوط به یک شخص

                var listPostIdCommission = postsofPersonCommission.Select(s => s.PostId).ToList(); // آی دی سمت های امضاکننده مربوط به کمیسیون شخص ورودی
                var listPostIdCouncil = postsofPersonCouncil.Select(s => s.PostId).ToList(); // آی دی سمت های امضاکننده مربوط به شورا شخص ورودی

                //////var listRowNumberComision = (from postPerson in postsofPersonCommission
                //////                             from signer in postPerson.Post.Signers.Where(s => s.RequestType == RequestType.Comision)
                //////                             select (int)signer.RowNumber).ToList(); // لیست شماره ردیف امضاکننده کمیسیون مربوط به شخص ورودی
                //////var listRowNumberCouncil = (from postPerson in postsofPersonCouncil
                //////                            from signer in postPerson.Post.Signers.Where(s => s.RequestType == RequestType.Council)
                //////                            select (int)signer.RowNumber).ToList(); // لیست شماره ردیف امضاکننده شورا مربوط به شخص ورودی

                List<long> listFieldofStudy;
                var allFieldofStudy = _fieldofStudyRepository.All();
                var request = new List<CartableViewModel>();
                foreach (var item in postsofPerson)
                {
                    var field = allFieldofStudy;
                    if (item.UniversityId != null)
                        field = field.Where(f => f.EducationalGroup.College.UniversityId == item.UniversityId);
                    else if (item.CollegeId != null)
                        field = field.Where(f => f.EducationalGroup.CollegeId == item.CollegeId);
                    else
                        field = field.Where(f => f.EducationalGroupId == item.EducationalGroupId);
                    listFieldofStudy = field.Select(s => s.Id).ToList();

                    foreach (var signer in item.Post.Signers)
                    {
                        request.AddRange(_requestRepository.Where(x => (x.RequestStatus == RequestStatus.InFlow &&
                                                                        x.RequestType == signer.RequestType && x.Archive == null) &&
                                                                       // در خواست درجریان
                                                                       (x.Cartables.Any(c => c.CartableValidation == CartableValidation.Valid)) &&
                                                                       // در کارتابل رکورد معتبر از این درخواست وجود داشته باشد
                                                                       (listFieldofStudy.Any(l => l.Equals(x.Person.Student.FieldofStudyId))) &&
                                                                        //درخواست در لیست رشته های تحصیلی کمیسیون مربوط به شخص ورودی باشد
                                                                        (!x.Cartables.Any(c => c.CartableValidation == CartableValidation.Valid &&
                                                                                 c.PostId == signer.PostId && c.RowNumber == signer.RowNumber)) &&
                                                                        // حداقل یک مورد از سمت های شخص در کارتابل رکورد معتبر نداشته باشد(یعنی امضا نداشته باشد)
                                                                        // یا رای موقت وجود داشته باشد
                                                                        ((ordinal != (int)Ordinal.Ordinal && signer.RowNumber <= 5) ||
                                                                         (x.Cartables.Any(c => c.CartableValidation ==
                                                                                 CartableValidation.Valid && c.RowNumber == signer.RowNumber - 1)))


                            ////        // در حالت قبلی مورد استفاده می شد که که در صدور رای ارجا به داشته ایم
                            ////         ((ordinal != (int)Ordinal.Ordinal && signer.RowNumber <= 5) ||
                            ////(x.Cartables.Any(c => c.CartableValidation ==
                            ////        CartableValidation.Valid && c.RowNumber == signer.RowNumber - 1 && c.ReferTo) ||
                            ////        (x.RequestType == RequestType.Council && signer.RowNumber == lastCouncilSigner &&
                            ////        x.Cartables.Any(c => c.CartableValidation ==
                            ////        CartableValidation.Valid && c.RowNumber == signer.RowNumber - 2 && !c.ReferTo))))

                            //////         &&// این مرحله قبلا برای مشمول بررسی می شد
                            //////(signer.RowNumber != (x.RequestType == RequestType.Comision
                            //////   ? lastCommissionSigners : lastCouncilSigners) ||
                            ////////مشخص میکند که درخواست در مرحله آخر میباشد یا خیر
                            //////(x.Vote != null && (x.RequestType != RequestType.Comision ||
                            //////        (x.Person.Profile.Gender == Gender.Male && x.Person.Student.MilitaryServiceStatus ==
                            //////        MilitaryServiceStatus.Included))))
                            )
                            // اگر درخواست در مرحله آخر باشد باید شرط صدور رای بررسی شود ، همچنین اگر کمیسیون باشد باید مشمول خدمت بررسی شود
                            .Include(i => i.Vote).Include(i => i.Commission).Include(i => i.Council)
                            .Include(i => i.Person).Include(i => i.Person.Profile).Include(i => i.Person.Student)
                            .Include(i => i.Person.Student.FieldofStudy.OrganizationStructureName)
                            .Select(req => new CartableViewModel
                            {
                                Request = req,
                                Person = req.Person,
                                Profile = req.Person.Profile,
                                Student = req.Person.Student,
                                Council = req.Council,
                                OrganizationStructureName = req.Person.Student.FieldofStudy.OrganizationStructureName,
                                Commission = req.Commission,
                                //این لیست کارتابل و لیست امضا کنندگان به دلیل اشکالی که در حالت غیر ترتیبی داشت فعلا غیر فعال شده است
                                //مثلا اگر شخصی هم سمت مدیر گروه و معاون آموزشی و اختیارات مدیر کل داشته باشد 
                                // به طوری که درخواست ربطی به مدیر گروه نداشته باشد یعنی زیر مجموعه آن نباشد
                                // در کارتابل درخواست در مرحله مدیر گروه نشان داده می شود که
                                // باید مدیر کل نشان داده شود
                                //Cartables =
                                //    req.Cartables.Where(c => c.CartableValidation == CartableValidation.Valid).ToList(),
                                //Signers = signers.Where(sel => sel.RequestType == req.RequestType &&
                                //                               (req.RequestType == RequestType.Comision
                                //                                   ? (listPostIdCommission.Except(req.Cartables.Where(
                                //                                       c => c.CartableValidation == CartableValidation.Valid)
                                //                                       .Select(s => s.PostId)).Any(l => l.Equals(sel.PostId)))
                                //                                   : (listPostIdCouncil.Except(
                                //                                       req.Cartables.Where(
                                //                                           c => c.CartableValidation == CartableValidation.Valid)
                                //                                           .Select(s => s.PostId)).Any(l => l.Equals(sel.PostId)))))
                                //    .ToList(),
                                Signers = signers.Where(s => s.Id == signer.Id).ToList(),
                                Signer = signers.FirstOrDefault(s => s.Id == signer.Id),
                                LastCommissionSigner = lastCommissionSigner,
                                LastCouncilSigner = lastCouncilSigner
                            }).ToList());
                    } //لیست رشته های تحصیلی که در زیر مجموعه سمت های کمیسیون مربوط به شخص ورودی وجود دارد
                }

                return new Tuple<bool, string, List<CartableViewModel>, ArgumentException>(true, "", request.OrderBy(i => i.Signer.RowNumber).DistinctBy(d => d.Request.Id).ToList(), null);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, List<CartableViewModel>, ArgumentException>(false, "", null, new ArgumentException("", exception));
            }
        }

        public int GetCartableCount(long userId)
        {
            try
            {
                var ordinal1 = _settingsRepository.All().Select(s => s.Ordinal).FirstOrDefault();
                var ordinal = (int)ordinal1; //برای مشخص کردن حالت ترتیبی یا غیر ترتیبی کارتابل
                var signers = _signerRepository.All().Include(i => i.Post).OrderBy(o => o.RowNumber); // لیست همه امضاکنندگان کمیسیون و شورا به ترتیب
                var lastCouncilSigner = signers.Where(w => w.RequestType == RequestType.Council).Select(s => s.RowNumber).ToList().LastOrDefault();

                var postsofPerson = _postPersonRepository.Where(p => p.PersonId == userId && p.Post.Signers.Any(s => s.RowNumber != 1)).Include(i => i.Post).Include(i => i.Post.Signers).ToList(); // همه سمت ها به همراه رکوردهای امضاکننده کمیسیون و شورا مربوط به یک شخص

                List<long> listFieldofStudy;
                var allFieldofStudy = _fieldofStudyRepository.All();
                var cartableCount = 0;
                foreach (var item in postsofPerson)
                {
                    var field = allFieldofStudy;
                    if (item.UniversityId != null)
                        field = field.Where(f => f.EducationalGroup.College.UniversityId == item.UniversityId);
                    else if (item.CollegeId != null)
                        field = field.Where(f => f.EducationalGroup.CollegeId == item.CollegeId);
                    else
                        field = field.Where(f => f.EducationalGroupId == item.EducationalGroupId);
                    listFieldofStudy = field.Select(s => s.Id).ToList();

                    cartableCount += item.Post.Signers.Sum(signer => _requestRepository.Where(x =>
                        (x.RequestStatus == RequestStatus.InFlow && x.RequestType == signer.RequestType && x.Archive == null) &&
                        (x.Cartables.Any(c => c.CartableValidation == CartableValidation.Valid)) &&
                        (listFieldofStudy.Any(l => l.Equals(x.Person.Student.FieldofStudyId)) &&
                        !x.Cartables.Any(c => c.CartableValidation == CartableValidation.Valid &&
                        c.PostId == signer.PostId && c.RowNumber == signer.RowNumber) && ((ordinal != (int)Ordinal.Ordinal && signer.RowNumber <= 5) ||
                        (x.Cartables.Any(c => c.CartableValidation == CartableValidation.Valid && c.RowNumber == signer.RowNumber - 1 && c.ReferTo) ||
                        (x.RequestType == RequestType.Council && signer.RowNumber == lastCouncilSigner &&
                        x.Cartables.Any(c => c.CartableValidation == CartableValidation.Valid && c.RowNumber == signer.RowNumber - 2 && !c.ReferTo)))))).Count());
                }
                return cartableCount;
            }
            catch (Exception exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// درج کارتابل
        /// Savechange
        /// انجام نمی شود
        /// </summary>
        /// <returns></returns>
        private bool InsertCartable(CartableStatus cartableStatus, long userId, long personIdOnBehalfof, long requestId, long postuserId, int rowNumber, bool referTo, string description)
        {
            // بررسی این که درخواست معتبر می باشد یا خیر
            var requestIsValid = _requestRepository.Where(s => s.Id == requestId && (rowNumber <= 1 ||
                s.RequestStatus != RequestStatus.Returned)).Include(i => i.Person.Profile)
                .Include(i => i.Person.Student).FirstOrDefault();
            if (requestIsValid == null) return false;
            var newCartable = new Cartable
            {
                RequestId = requestId,
                PersonId = userId,
                PostId = postuserId,
                RowNumber = rowNumber,
                ReferTo = referTo,
                Series = _cartableRepository.Where(c => c.RequestId == requestId && c.RowNumber == 1).Count() + (rowNumber == 1 ? 1 : 0),
                CartableStatus = cartableStatus,
                Description = description,
                PersonIdOnBehalfof = personIdOnBehalfof,
                CartableValidation = cartableStatus == CartableStatus.Returned ? CartableValidation.Invalid : CartableValidation.Valid// کلا در خواست ها برگشی در کارتابل نا معتبر باید باشد- در جدول کارتابل درخواست معتبر و برگشت زده نداریم
            };
            //_cartableRepository.AddOrUpdate(c => new { c.RequestId, c.PostId, c.CartableStatus, c.CartableValidation, c.RowNumber, c.Series }, newCartable);
            _cartableRepository.Add(newCartable);

            if (cartableStatus == CartableStatus.Verdict)
            {
                //if (VoteStatus == VoteStatus.Temporary)
                //    cartableStatus = CartableStatus.Confirmed;
                //else
                //{
                if (requestIsValid.RequestType == RequestType.Comision)
                {
                    if (referTo)
                        cartableStatus = CartableStatus.Confirmed;
                }
                else
                    cartableStatus = CartableStatus.Confirmed; // چون شورا است باید درخواست در جریان شود 
                                                               //}
            }
            requestIsValid.RequestStatus = new Request().ConvertCartableStatustoRequestStatus(cartableStatus);
            _requestRepository.Update(requestIsValid);
            return true;
        }

        public Tuple<bool, string, string> Confirmation(long userId, long personIdOnBehalfof, long requestId, long postuserId, int rowNumber, string description)
        {
            try
            {
                var valid = InsertCartable(CartableStatus.Confirmed, userId, personIdOnBehalfof, requestId, postuserId, rowNumber, true, description);
                if (!valid) return new Tuple<bool, string, string>(false, "خطا در تایید درخواست", null);
                _unitOfWork.SaveChanges();
                var signature = _personelRepository.Where(p => p.PersonId == userId).FirstOrDefault()?.Signature;
                return new Tuple<bool, string, string>(true, "درخواست تایید شد", signature);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, string>(false, "عملیات با مشکل مواجه شده است", null);
            }

        }
        public Tuple<bool, string> Returned(long userId, long personIdOnBehalfof, long requestId, long postuserId, int rowNumber, string description)
        {
            try
            {
                var valid = InsertCartable(CartableStatus.Returned, userId, personIdOnBehalfof, requestId, postuserId, rowNumber, true, description);
                if (!valid) return new Tuple<bool, string>(false, "درخواست درجریان نمی باشد");
                // تمام در خواست های ما قبل باید به حالت نا معتبری بر گردد
                var upobjects =
                    _cartableRepository.Where(
                        qu => qu.RequestId == requestId && qu.CartableValidation == CartableValidation.Valid).ToList();
                // خاصیت ارجاعی دارد
                //  _cartableRepository.Update(upobjects);
                //  _cartableRepository.Update(qu => qu.RequestId == requestId, up => new Cartable { CartableValidation = CartableValidation.Invalid });
                upobjects.ForEach(up => up.CartableValidation = CartableValidation.Invalid);

                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "درخواست برگشت داده شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "عملیات با مشکل مواجه شده است");
            }

        }
        public Tuple<bool, string> AgreetoVote(long userId, long personIdOnBehalfof, long requestId, long postuserId, int rowNumber, string description)
        {
            try
            {
                bool referTo = true;
                // اگر شورا باشد و در مرحله 6 باشید نباید ارجاع داشته باشد
                if (rowNumber == 6)
                    referTo = !_requestRepository.Contains(d => d.Id == requestId && d.RequestType == RequestType.Council);

                var valid = InsertCartable(CartableStatus.AgreetoVote, userId, personIdOnBehalfof, requestId, postuserId, rowNumber, referTo, description);
                if (!valid) return new Tuple<bool, string>(false, "خطا در انجام عملیات!");
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "درخواست تایید شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "عملیات با مشکل مواجه شده است!");
            }

        }
        public Tuple<bool, string> OppositiontoVote(long userId, long personIdOnBehalfof, long requestId, long postuserId, int rowNumber, string description)
        {
            try
            {
                bool referTo = true;
                // اگر شورا باشد و در مرحله 6 باشید نباید ارجاع داشته باشد
                if (rowNumber == 6)
                    referTo = !_requestRepository.Contains(d => d.Id == requestId && d.RequestType == RequestType.Council);

                var valid = InsertCartable(CartableStatus.OppositiontoVote, userId, personIdOnBehalfof, requestId, postuserId, rowNumber, referTo, description);
                if (!valid) return new Tuple<bool, string>(false, "خطا در انجام عملیات!");
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "درخواست تایید شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "عملیات با مشکل مواجه شده است!");
            }
        }
        public Tuple<bool, string> VoteTemporary(VoteModel voteModel)
        {
            try
            {
                _voteRepository.AddOrUpdate(v => v.RequestId, new Vote { RequestId = voteModel.RequestId, PersonId = voteModel.PersonId, PostId = voteModel.PostId, VoteText = voteModel.VoteText, ReferText = voteModel.VoteDescription, VoteType = voteModel.VoteType, VoteStatus = voteModel.VoteStatus, DateVote = voteModel.DateVote.ToMiladiDate() });
                if (voteModel.RequestType == RequestType.Comision)
                {
                    var commission = _commissionRepository.Find(c => c.RequestId == voteModel.RequestId);
                    if (commission != null)
                    {// اول شماره کمیسیون در متد ایجاد درخواست بود
                     // بر اساس تغییرات خواسته شده شماره کیسیون باید در مرحله صدور رای بیاید
                        commission.CommissionNumber = voteModel.DetailRequestModel.CommissionNumber;
                        _commissionRepository.Update(commission);
                    }
                }
                else
                {
                    var council = _councilRepository.Find(c => c.RequestId == voteModel.RequestId);
                    if (council != null)
                    {
                        // این شماره  CommissionNumber
                        // DetailRequestModel در مدل 
                        // برای کمیسیون و شورای یکی می باشد 
                        council.CouncilNumber = voteModel.DetailRequestModel.CommissionNumber;
                        _councilRepository.Update(council);
                    }
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "صدور رای موقت با موفقیت انجام شده است");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "عملیات با مشکل مواجه شده است");
            }

        }
        public Tuple<bool, string> VotePermanent(VoteModel voteModel)
        {
            try
            {
                //var request = _requestRepository.Where(r => r.Id == voteModel.RequestId).Include(i => i.Person)
                //    .Include(i => i.Person.Profile).Include(i => i.Person.Student).FirstOrDefault();
                //ابتدا در کارتابل ذخیره شود
                var valid = InsertCartable(CartableStatus.Verdict, voteModel.PersonId, voteModel.PersonId, voteModel.RequestId, voteModel.PostId, voteModel.RowNumber, (voteModel.RequestType == RequestType.Council || voteModel.ReferTo), voteModel.VoteDescription);

                if (!valid)
                    return new Tuple<bool, string>(false,
                        "درخواست مورد نظر در جریان نمی باشد و صدور رای امکان پذیر نیست");
                // رای صادر شود
                _voteRepository.AddOrUpdate(v => v.RequestId, new Vote { RequestId = voteModel.RequestId, PersonId = voteModel.PersonId, PostId = voteModel.PostId, VoteText = voteModel.VoteText, ReferText = voteModel.VoteDescription, VoteType = voteModel.VoteType, VoteStatus = voteModel.VoteStatus, DateVote = voteModel.DateVote.ToMiladiDate() });
                //هر کمیسیون یک شماره جلسه دارد که باید برای هر کمیسیون شماره ردیفی که در چاپ می آید مشخص شود
                // در واقه با ین شماره می توان ردیف کمیسیون را فهمید 
                if (voteModel.RequestType == RequestType.Comision)
                {
                    var commission = _commissionRepository.Find(c => c.RequestId == voteModel.RequestId);
                    if (commission != null)
                    {
                        commission.RowNumber =
                            _commissionRepository.Where(c => c.CommissionNumber == commission.CommissionNumber)
                                .Select(s => s.RowNumber).DefaultIfEmpty(0).Max() + 1;
                        // اول شماره کمیسیون در متد ایجاد درخواست بود
                        // بر اساس تغییرات خواسته شده شماره کیسیون باید در مرحله صدور رای بیاید
                        commission.CommissionNumber = voteModel.DetailRequestModel.CommissionNumber;
                        _commissionRepository.Update(commission);
                    }
                }
                else
                {
                    var council = _councilRepository.Find(c => c.RequestId == voteModel.RequestId);
                    if (council != null)
                    {
                        council.RowNumber =
                            _councilRepository.Where(c => c.CouncilNumber == council.CouncilNumber)
                                .Select(s => s.RowNumber).DefaultIfEmpty(0).Max() + 1;
                        // این شماره  CommissionNumber
                        // DetailRequestModel در مدل 
                        // برای کمیسیون و شورای یکی می باشد 
                        council.CouncilNumber = voteModel.DetailRequestModel.CommissionNumber;
                        _councilRepository.Update(council);
                    }
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "صدور رای با موفقیت انجام شده است");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "عملیات با مشکل مواجه شده است");
            }

        }
        public Tuple<bool, string, FollowUpModel> FollowUpRequest(long requestId)
        {
            try
            {
                var followUpModel = _requestRepository.Where(x => x.Id == requestId)
                    .Include(i => i.Vote)
                    .Include(i => i.Archive)
                    .Include(i => i.Commission)
                    .Include(i => i.Council)
                    .Include(i => i.Person)
                    .Include(i => i.Person.Profile)
                    .Include(i => i.Person.Student)
                    .Include(i => i.Person.Student.FieldofStudy)
                    .Include(i => i.Person.Student.FieldofStudy.OrganizationStructureName)
                    //.Include(i => i.Cartables)//.Select(s => new { Person = s.Person, Post = s.Post, Profile = s.Person.Profile, Personel = s.Person.Personel })
                    .Select(req => new FollowUpModel
                    {
                        Avatar = req.Person.Profile.Avatar,
                        RequestId = req.Id,
                        RequestDate = req.CreatedDate,
                        CommissionCouncilNumber = req.Commission != null ? req.Commission.CommissionNumber : req.Council.CouncilNumber,
                        CommissionCouncilDate = req.Commission != null ? req.Commission.Date : req.Council.Date,
                        PersonId = req.PersonId,
                        NameFamily = req.Person.Profile.Name + " " + req.Person.Profile.Family,
                        StudentNumber = req.Person.Student.StudentNumber,
                        FieldofStudy = req.Person.Student.FieldofStudy.OrganizationStructureName.Name,
                        Grade = req.Person.Student.Grade,
                        FollowUpDetails = req.Cartables.Select(c => new FollowUpDetailsModel
                        {
                            PersonId = c.PersonId,
                            NameFamily = c.Person.Profile.Name + " " + c.Person.Profile.Family,
                            Avatar = c.Person.Profile.Avatar,
                            PostId = c.PostId,
                            PostName = c.Post.Name,
                            Signature = c.Person.Personel.Signature,
                            RowNumber = c.RowNumber,
                            CartableStatus = c.CartableStatus,
                            CartableValidation = c.CartableValidation,
                            Description = c.Description
                        }).ToList()

                    }).FirstOrDefault();
                return new Tuple<bool, string, FollowUpModel>(true, "", followUpModel);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, FollowUpModel>(false, "عملیات با مشکل مواجه شده است", null);
            }
        }
        public Tuple<bool, string, VoteModel> GetVote(long universityId, long voteId, RequestType requestType, AddressUrlFile addressUrlFile)
        {
            try
            {
                //var request = _requestRepository.Where(r => r.Id == voteId).Select(s => new { s.RequestType, s.Commission, s.Council,s.Vote }).FirstOrDefault();
                var detailRequestModel = _requestService.DetailRequest(voteId, addressUrlFile);
                if (!detailRequestModel.Item1)
                    return new Tuple<bool, string, VoteModel>(false, "عملیات با مشکل مواجه شده است", null);

                DateTime? dateVote;
                if (requestType == RequestType.Comision)
                {
                    detailRequestModel.Item3.CommissionNumber = _commissionService.GetCommissionNumber(universityId).Item1;
                    dateVote =
                        _voteRepository.Where(w => w.Request.Commission.CommissionNumber == detailRequestModel.Item3.CommissionNumber).
                        Select(s => s.DateVote).FirstOrDefault();
                }
                else
                {
                    detailRequestModel.Item3.CommissionNumber = _councilService.GetCouncilNumber(universityId).Item1;
                    dateVote = _voteRepository.Where(w => w.Request.Council.CouncilNumber == detailRequestModel.Item3.CommissionNumber).
                        Select(s => s.DateVote).FirstOrDefault();
                }

                var vote = _requestRepository.Where(x => x.Id == voteId)
                    .Select(v => new VoteModel
                    {
                        PersonId = v.Vote != null ? v.Vote.PersonId : 0,
                        PersonIdOnBehalfof = v.Vote != null ? v.Vote.PersonId : 0,
                        PostId = v.Vote != null ? v.Vote.PostId : 0,
                        RequestId = voteId,
                        DateVote = v.Vote != null ? v.Vote.DateVote : dateVote ?? DateTime.Now,
                        VoteText = v.Vote != null ? v.Vote.VoteText : "",
                        VoteDescription = v.Vote != null ? v.Vote.ReferText : "",
                        VoteType = v.Vote != null ? v.Vote.VoteType : 0,
                        VoteStatus = v.Vote != null ? v.Vote.VoteStatus : 0,
                        RequestType = v.RequestType,
                        Included = (v.RequestType == RequestType.Comision && v.Person.Profile.Gender == Gender.Male &&
                                   v.Person.Student.MilitaryServiceStatus == MilitaryServiceStatus.Included),

                        Name = v.Person.Profile.Name,
                        Family = v.Person.Profile.Family,
                        StudentNumber = v.Person.Student.StudentNumber,
                        FieldofStudy = v.Person.Student.FieldofStudy.OrganizationStructureName.Name,
                        FieldofStudyId = v.Person.Student.FieldofStudyId,
                        Grade = v.Person.Student.Grade.ToString(),
                        NationalCode = v.Person.Profile.NationalCode
                        //Description = v.Request.Cartables.FirstOrDefault(c =>
                        //    c.CartableStatus == CartableStatus.Verdict &&
                        //    c.CartableValidation == CartableValidation.Valid).Description
                    }).FirstOrDefault();
                if (vote == null)
                    return new Tuple<bool, string, VoteModel>(false, "عملیات با مشکل مواجه شده است", null);
                vote.DetailRequestModel = detailRequestModel.Item3;
                return new Tuple<bool, string, VoteModel>(true, "", vote);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, VoteModel>(false, "عملیات با مشکل مواجه شده است", null);
            }
        }
        public Tuple<bool, string, Person> GetPersonInSigners(int rownumber, RequestType requestType, long fieldofStudyId)
        {
            try
            {
                var post = _signerRepository.Where(s => s.RowNumber == rownumber && s.RequestType == requestType)
                    .Select(s => s.Post).Include(i => i.PostPersons.Select(per => per.Person))
                    .Include(s => s.PostPersons.Select(sp => sp.University.Colleges.Select(cs => cs.EducationalGroups.Select(se => se.FieldofStudies)))).FirstOrDefault();
                var postId = _signerRepository.Where(s => s.RowNumber == rownumber && s.RequestType == requestType).Select(s => s.PostId).FirstOrDefault();

                if (post == null)
                    return new Tuple<bool, string, Person>(false, "داده نامعتبر در عملیات وجود دارد!", null);
                var person = new Person();
                switch (post.PostType)
                {
                    case PostType.University:
                        //person = post.PostPersons.FirstOrDefault(p => p.University.Colleges.Any(c => c.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId))))?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.University.Colleges.Any(
                                c => c.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId)))).Select(s => s.Person).FirstOrDefault();
                        break;
                    case PostType.College:
                        //person = post.PostPersons.FirstOrDefault(p => p.College.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId)))?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.College.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId))).Select(s => s.Person).FirstOrDefault();
                        break;
                    case PostType.EducationalGroup:
                        //person = post.PostPersons.FirstOrDefault(p => p.EducationalGroup.FieldofStudies.Any(f => f.Id == fieldofStudyId))?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.EducationalGroup.FieldofStudies.Any(f => f.Id == fieldofStudyId)).Select(s => s.Person).FirstOrDefault();
                        break;
                    case PostType.FieldofStudy:
                        //person = post.PostPersons.FirstOrDefault(p => p.FieldofStudyId == fieldofStudyId)?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.FieldofStudyId == fieldofStudyId).Select(s => s.Person).FirstOrDefault();
                        break;
                    default:
                        person = null;
                        break;
                }
                if (person == null)
                    return new Tuple<bool, string, Person>(false, "پرسنل مسئول پرونده یافت نشد!", null);

                return new Tuple<bool, string, Person>(true, "", person);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, Person>(false, "عملیات با مشکل مواجه شده است!", null);
            }
        }
        public Tuple<bool, string, Person> GetPersonInSigners(long postId, long fieldofStudyId)
        {
            try
            {
                var post = _postRepository.Where(p => p.Id == postId).Include(i => i.PostPersons.Select(per => per.Person))
                    .Include(s => s.PostPersons.Select(sp => sp.University.Colleges.Select(cs => cs.EducationalGroups.Select(se => se.FieldofStudies)))).FirstOrDefault();
                if (post == null)
                    return new Tuple<bool, string, Person>(false, "داده نامعتبر در عملیات وجود دارد!", null);
                var person = new Person();
                switch (post.PostType)
                {
                    case PostType.University:
                        //person = post.PostPersons.FirstOrDefault(p => p.University.Colleges.Any(c => c.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId))))?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.University.Colleges.Any(
                                c => c.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId)))).
                                Select(s => s.Person).FirstOrDefault();
                        break;
                    case PostType.College:
                        //person = post.PostPersons.FirstOrDefault(p => p.College.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId)))?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.College.EducationalGroups.Any(a => a.FieldofStudies.Any(f => f.Id == fieldofStudyId))).
                            Select(s => s.Person).FirstOrDefault();
                        break;
                    case PostType.EducationalGroup:
                        //person = post.PostPersons.FirstOrDefault(p => p.EducationalGroup.FieldofStudies.Any(f => f.Id == fieldofStudyId))?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.EducationalGroup.FieldofStudies.Any(f => f.Id == fieldofStudyId)).
                            Select(s => s.Person).FirstOrDefault();
                        break;
                    case PostType.FieldofStudy:
                        //person = post.PostPersons.FirstOrDefault(p => p.FieldofStudyId == fieldofStudyId)?.Person;
                        person = _postPersonRepository.Where(p => p.PostId == postId && p.FieldofStudyId == fieldofStudyId).
                            Select(s => s.Person).FirstOrDefault();
                        break;
                    default:
                        person = null;
                        break;
                }
                if (person == null)
                    return new Tuple<bool, string, Person>(false, "پرسنل مسئول پرونده یافت نشد!", null);

                return new Tuple<bool, string, Person>(true, "", person);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, Person>(false, "عملیات با مشکل مواجه شده است!", null);
            }
        }

    }
}
