using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;

namespace Comision.Service.ImplementationService
{
    public class CouncilService : ICouncilService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IMemberMasterRepository _memberMasterRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ISignerRepository _signerRepository;
        private readonly ICartableRepository _cartableRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly ICouncilRepository _councilRepository;
        public CouncilService(IUnitOfWork unitOfWork, IStudentRepository studentRepository,
            IProfileRepository profileRepository, IRequestRepository requestRepository,
            IMemberMasterRepository memberMasterRepository, IPersonRepository personRepository,
            ISignerRepository signerRepository, ICartableRepository cartableRepository,
            ISettingsRepository settingsRepository, ICouncilRepository councilRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
            _profileRepository = profileRepository;
            _requestRepository = requestRepository;
            _memberMasterRepository = memberMasterRepository;
            _personRepository = personRepository;
            _signerRepository = signerRepository;
            _cartableRepository = cartableRepository;
            _settingsRepository = settingsRepository;
            _councilRepository = councilRepository;
        }

        /// <summary>
        /// ذخیره درخواست شورا به همراه پروفایل و دانشجو و شخص
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public Tuple<bool, string, long> AddCouncilRequest(CouncilModel requestModel, long universityId)
        {
            try
            {
                //واکشی درخواست اگر وجود دارد
                var request = _requestRepository.Where(i => i.Id == requestModel.Id).Include(i => i.Council)
                    .Include(i => i.MemberMaster).FirstOrDefault() ?? new Request();
                request.RequestType = requestModel.RequestType;
                request.NumberofRemainingUnits = requestModel.NumberofRemainingUnits;
                request.NumberofSpentUnits = requestModel.NumberofSpentUnits;
                request.RequestStatus = requestModel.RequestStatus;
                request.Description = requestModel.Description;
                request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Council)
                    .OrderByDescending(p => p.Id).FirstOrDefault();

                var x = new PersianCalendar();
                requestModel.Date =
                    x.ToDateTime(requestModel.Date.Year, requestModel.Date.Month, requestModel.Date.Day, 0, 0, 0, 0, 0);
                //درج درخواست به همراه شورا
                if (request.Id > 0)
                {
                    request.Council.CouncilNumber = requestModel.CouncilNumber;
                    request.Council.Date = requestModel.Date;
                    request.Council.Description = requestModel.Description;
                    request.Council.ProblemText = requestModel.ProblemsCouncil;
                    _requestRepository.Update(request);
                }
                else
                {
                    request.PersonId = requestModel.PersonId;
                    request.Council = new Council
                    {
                        CouncilNumber = requestModel.CouncilNumber,
                        Date = requestModel.Date,
                        ProblemText = requestModel.ProblemsCouncil,
                        Description = requestModel.Description
                    };
                    _requestRepository.Add(request);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, long>(true, "عملیات ثبت به درستی انجام شد", request.Id);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, long>(false, "خطا در ثبت درخواست", 0);
            }
        }
        //public Tuple<bool, string, long> AddCouncilRequest(CouncilModel requestModel, long universityId)
        //{
        //    try
        //    {
        //        //واکشی دانشجو اگر وجود دارد وگرنه ایجاد می شود
        //        var student =
        //            _studentRepository.Where(s => s.StudentNumber == requestModel.StudentNumber)
        //                .Include(i => i.Person).Include(i => i.Person.Profile).FirstOrDefault() ??
        //            new Student();
        //        student.StudentNumber = requestModel.StudentNumber;
        //        student.FieldofStudyId = requestModel.FieldofStudyId;
        //        student.Grade = requestModel.Grade;
        //        student.MilitaryServiceStatus = requestModel.MilitaryServiceStatus;

        //        //واکشی پروفایل اگر وجود دارد
        //        Profile profile;
        //        if (student.Person == null)
        //        {
        //            profile =
        //             _profileRepository.Where(p => p.NationalCode == requestModel.NationalCode).FirstOrDefault() ??
        //            new Profile();
        //        }
        //        else
        //        {
        //            profile = student.Person.Profile;
        //        }
        //        profile.Name = requestModel.Name;
        //        profile.Family = requestModel.Family;
        //        profile.NationalCode = requestModel.NationalCode;
        //        profile.Gender = requestModel.Gender;

        //        //واکشی درخواست اگر وجود دارد
        //        var request = _requestRepository.Where(i => i.Id == requestModel.Id).Include(i => i.Council)
        //            .Include(i => i.MemberMaster).FirstOrDefault() ?? new Request();
        //        request.RequestType = requestModel.RequestType;
        //        request.NumberofRemainingUnits = requestModel.NumberofRemainingUnits;
        //        request.NumberofSpentUnits = requestModel.NumberofSpentUnits;
        //        request.RequestStatus = requestModel.RequestStatus;
        //        request.Description = requestModel.Description;
        //        request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Council)
        //            .OrderByDescending(p => p.Id).FirstOrDefault();

        //        long personId;
        //        if (profile.PersonId == 0)
        //        {
        //            var person = new Person
        //            {
        //                Active = true,
        //                UniversityId = universityId,
        //                Profile = profile,
        //                Student = student
        //            };
        //            _personRepository.Add(person);
        //            personId = person.Id;
        //        }
        //        else
        //        {
        //            personId = profile.PersonId;
        //            student.PersonId = profile.PersonId;
        //            _studentRepository.AddOrUpdate(s => s.PersonId, student);
        //        }

        //        var x = new PersianCalendar();
        //        requestModel.Date =
        //            x.ToDateTime(requestModel.Date.Year, requestModel.Date.Month, requestModel.Date.Day, 0, 0, 0, 0, 0);
        //        //درج درخواست به همراه شورا
        //        if (request.Id > 0)
        //        {
        //            request.Council.CouncilNumber = requestModel.CouncilNumber;
        //            request.Council.Date = requestModel.Date;
        //            request.Council.Description = requestModel.Description;
        //            request.Council.ProblemText = requestModel.ProblemsCouncil;
        //            _requestRepository.Update(request);
        //        }
        //        else
        //        {
        //            request.PersonId = personId;
        //            request.Council = new Council
        //            {
        //                CouncilNumber = requestModel.CouncilNumber,
        //                Date = requestModel.Date,
        //                ProblemText = requestModel.ProblemsCouncil,
        //                Description = requestModel.Description
        //            };
        //            _requestRepository.Add(request);
        //        }
        //        _unitOfWork.SaveChanges();
        //        return new Tuple<bool, string, long>(true, "عملیات ثبت به درستی انجام شد", request.Id);
        //    }
        //    catch (Exception exception)
        //    {
        //        return new Tuple<bool, string, long>(false, "خطا در ثبت درخواست", 0);
        //    }
        //}

        /// <summary>
        /// ذخیره درخواست شورا به همراه پروفایل و دانشجو و شخص
        /// </summary>
        /// <param name="councilModel"></param>
        /// <returns></returns>
        public Tuple<bool, string, long> UpdateCouncilRequest(CouncilModel councilModel)
        {
            try
            {
                //واکشی درخواست اگر وجود دارد
                var request = _requestRepository.Where(i => i.Id == councilModel.Id).Include(i => i.Council)
                    .Include(i => i.MemberMaster).FirstOrDefault() ?? new Request();
                request.NumberofRemainingUnits = councilModel.NumberofRemainingUnits;
                request.NumberofSpentUnits = councilModel.NumberofSpentUnits;
                request.Description = councilModel.Description;
                request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Council)
                    .OrderByDescending(p => p.Id).FirstOrDefault();

                var x = new PersianCalendar();
                councilModel.Date =
                    x.ToDateTime(councilModel.Date.Year, councilModel.Date.Month, councilModel.Date.Day, 0, 0, 0, 0, 0);
                //درج درخواست به همراه شورا
                request.Council.CouncilNumber = councilModel.CouncilNumber;
                request.Council.Date = councilModel.Date;
                request.Council.Description = councilModel.Description;
                request.Council.ProblemText = councilModel.ProblemsCouncil;
                _requestRepository.Update(request);

                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, long>(true, "عملیات ثبت به درستی انجام شد", request.Id);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, long>(false, "خطا در ثبت درخواست", 0);
            }
        }
        //public Tuple<bool, string, long> UpdateCouncilRequest(CouncilModel councilModel)
        //{
        //    try
        //    {
        //        var person =
        //            _personRepository.Where(s => s.Id == councilModel.PersonId).Include(i => i.Profile)
        //                .Include(i => i.Student).FirstOrDefault();
        //        person.Student.StudentNumber = councilModel.StudentNumber;
        //        person.Student.FieldofStudyId = councilModel.FieldofStudyId;
        //        person.Student.Grade = councilModel.Grade;
        //        person.Student.MilitaryServiceStatus = councilModel.MilitaryServiceStatus;

        //        person.Profile.Name = councilModel.Name;
        //        person.Profile.Family = councilModel.Family;
        //        person.Profile.NationalCode = councilModel.NationalCode;

        //        _studentRepository.Update(person.Student);
        //        _profileRepository.Update(person.Profile);

        //        //واکشی درخواست اگر وجود دارد
        //        var request = _requestRepository.Where(i => i.Id == councilModel.Id).Include(i => i.Council)
        //            .Include(i => i.MemberMaster).FirstOrDefault() ?? new Request();
        //        request.NumberofRemainingUnits = councilModel.NumberofRemainingUnits;
        //        request.NumberofSpentUnits = councilModel.NumberofSpentUnits;
        //        request.Description = councilModel.Description;
        //        request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Council)
        //            .OrderByDescending(p => p.Id).FirstOrDefault();

        //        var x = new PersianCalendar();
        //        councilModel.Date =
        //            x.ToDateTime(councilModel.Date.Year, councilModel.Date.Month, councilModel.Date.Day, 0, 0, 0, 0, 0);
        //        //درج درخواست به همراه شورا
        //        request.Council.CouncilNumber = councilModel.CouncilNumber;
        //        request.Council.Date = councilModel.Date;
        //        request.Council.Description = councilModel.Description;
        //        request.Council.ProblemText = councilModel.ProblemsCouncil;
        //        _requestRepository.Update(request);

        //        _unitOfWork.SaveChanges();
        //        return new Tuple<bool, string, long>(true, "عملیات ثبت به درستی انجام شد", request.Id);
        //    }
        //    catch (Exception exception)
        //    {
        //        return new Tuple<bool, string, long>(false, "خطا در ثبت درخواست", 0);
        //    }
        //}

        /// <summary>
        /// این متد اطلاعات مربوط به کمیسیون و پروفایل و دانشجویی یک شخص خاص را می دهد
        /// </summary>
        /// <param name="councilId"></param>
        /// <returns></returns>
        public Tuple<bool, string, CouncilModel> FindInfoCouncil(long councilId)
        {
            try
            {
                var councilModel = (from p in _requestRepository.Where(c => c.Id == councilId && c.RequestType == RequestType.Council)
                                        //.Include(i => i.Council).Include(i => i.Person).Include(i => i.Person.Student).Include(i => i.Person.Profile)
                                    select new CouncilModel
                                    {
                                        Id = councilId,
                                        NumberofRemainingUnits = p.NumberofRemainingUnits,
                                        NumberofSpentUnits = p.NumberofSpentUnits,
                                        RequestType = p.RequestType,
                                        RequestStatus = p.RequestStatus,

                                        CouncilNumber = p.Council.CouncilNumber,
                                        Date = p.Council.Date,
                                        Description = p.Council.Description,
                                        ProblemsCouncil = p.Council.ProblemText,

                                        PersonId = p.PersonId,
                                        NameFamily = p.Person.Profile.Name + " " + p.Person.Profile.Family,
                                        NationalCode = p.Person.Profile.NationalCode,

                                        StudentNumber = p.Person.Student.StudentNumber,
                                        FieldofStudyId = p.Person.Student.FieldofStudyId,
                                        FieldofStudy = p.Person.Student.FieldofStudy.OrganizationStructureName.Name,
                                        Gender = p.Person.Profile.Gender,
                                        Grade = p.Person.Student.Grade,
                                        MilitaryServiceStatus = p.Person.Student.MilitaryServiceStatus,
                                        NumberofVotesCouncil = 0 //p.Requests.Count(r => r.Council != null)
                                    }).FirstOrDefault();
                if (councilModel == null)
                    return new Tuple<bool, string, CouncilModel>(false, "رکورد کمیسیون نظر یافت نشد!", null);
                councilModel.NumberofVotesCouncil =
                    _councilRepository.Where(c => c.Request.PersonId == councilModel.PersonId).Count();
                councilModel.GenderString = councilModel.Gender.GetDescription();
                councilModel.GradeString = councilModel.Grade.GetDescription();
                councilModel.MilitaryServiceStatusString = councilModel.MilitaryServiceStatus.GetDescription();

                return new Tuple<bool, string, CouncilModel>(true, "", councilModel);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string, CouncilModel>(false, "خطا در انجام عملیات!", null);
            }
        }

        /// <summary>
        /// شماره کمیسیون ارسال می شود
        /// </summary>
        /// <param name="universityId">ایدی دانشگاه</param>
        /// <returns>اگر متد با خطا مواجه شود شماره -1 ارسال می شود</returns>
        public Tuple<long, string> GetCouncilNumber(long universityId)
        {
            try
            {
                var settingsUniversity = _settingsRepository.Where(s => s.UniversityId == universityId, sel => new Settings { CouncilNumberRepetitions = sel.CouncilNumberRepetitions }).FirstOrDefault();
                //if (settingsUniversity == null)// اگر تنظیمات در جدول وجود داشت
                //    return new Tuple<long, string>(-1, "رکورد تنظیمات در جدول وجود نداشت و به طور پیش فرض شماره کمیسیون -1 در نظر گرفته شده است");
                //  var lastCommissionNumber = _commissionRepository.Where(s => s.Request.Person.UniversityId == universityId).Max(b => b.RequestId);

                // اخرین کمیسیونی که برای آن رای صادر شده است
                var lastcouncilvote = _councilRepository.Where(s => s.Request.Person.UniversityId == universityId && s.Request.Vote != null, sel => new Council { RequestId = sel.RequestId, CouncilNumber = sel.CouncilNumber }).OrderByDescending(o => o.RequestId).FirstOrDefault();
                if (lastcouncilvote == null)// اگرکمیسیونی ذخیره نشده است
                    return new Tuple<long, string>(1, "کمیسیون ذخیره نشده است و شماره  شورا از 1 شروع خواهد شد");

                // تعداد کمیسیون هایی که برای آنها رای صادر شده است و شماره آن برابر با اخرین کمیسیونی است که برای آن رای صادر شد
                var countInsertrequest = _councilRepository.Where(s => s.Request.Person.UniversityId == universityId && s.Request.Vote != null && s.CouncilNumber == lastcouncilvote.CouncilNumber).Count();

                if (settingsUniversity == null || countInsertrequest >= settingsUniversity.CouncilNumberRepetitions)
                    return new Tuple<long, string>(lastcouncilvote.CouncilNumber + 1, "شماره کمیسیون بیشتر از حد کجاز شده است و یک شماره جدید اختصاص داده شده است");
                return new Tuple<long, string>(lastcouncilvote.CouncilNumber, "شماره کمیسیون هنوز به مجاز نرسیده است");
            }
            catch (Exception ex)
            {
                return new Tuple<long, string>(-1, "خطا در انجام عملیات!");
            }
        }
        public Tuple<bool, string> IsValidCouncilNumber(long universityId, long CouncilNumber)
        {
            try
            {
                var settingsUniversity = _settingsRepository.Where(s => s.UniversityId == universityId, sel => new Settings { CouncilNumberRepetitions = sel.CouncilNumberRepetitions }).FirstOrDefault();
                int countInsertrequest = _councilRepository.Where(s => s.Request.Person.UniversityId == universityId && s.CouncilNumber == CouncilNumber).Count();

                if (settingsUniversity != null)// اگر تنظیمات در جدول وجود داشت
                {
                    if (countInsertrequest < settingsUniversity.CouncilNumberRepetitions)// اگر هنوز به تعداد تکرار مورد نظر نرسیده ایم                    
                        return new Tuple<bool, string>(true, "شماره شورا معتبر می باشد");
                    return new Tuple<bool, string>(false, "شماره شورا بیشتر از حد مجاز می باشد ");
                }
                return new Tuple<bool, string>(false, "لطفا تنظیمات را بررسی نمایید ");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات!");
            }
        }

        /// <summary>
        /// تعداد دفعات استفاده از شورای آموزشی
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public long GetNumberofVotesCouncil(long studentId)
        {
            try
            {
                var count =
                    _requestRepository.Where(i => i.RequestType == RequestType.Council && i.PersonId == studentId)
                        .Count();

                return count;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public Tuple<bool, string, List<Council>> GetCouncilsProfileStudent(string searcvalue, SearchType searchtype)
        {
            try
            {
                Int64 number = (searchtype == SearchType.FileNumber || searchtype == SearchType.StudentNumber)
                    ? Convert.ToInt64(searcvalue):0;

                var query =
                    _councilRepository.Where(
                        s =>
                            (searchtype == SearchType.StudentNumber)
                                ? s.Request.Person.Student.StudentNumber == number
                                : (searchtype == SearchType.StudentNameFamili)
                                    ? (s.Request.Person.Profile.Name + " " + s.Request.Person.Profile.Family).Contains(searcvalue)
                                      
                                    : (searchtype == SearchType.FileNumber) && s.CouncilNumber == number);
                
                var councils = query.Include(i => i.Request.Vote)
                    .Include(i => i.Request.Person.Profile).Include(i => i.Request.Person.Student).ToList();
                return new Tuple<bool, string, List<Council>>(true, "", councils);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, List<Council>>(false, "خطا در انجام عملیات!", null);
            }
        }
    }
}
