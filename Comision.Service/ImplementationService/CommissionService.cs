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
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Comision.Service.Enum;

namespace Comision.Service.ImplementationService
{
    public class CommissionService : ICommissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IMemberMasterRepository _memberMasterRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICommissionSpecialEducationRepository _commissionSpecialEducationRepository;
        private readonly ICommissionRepository _commissionRepository;
        private readonly IPostPersonRepository _postPersonRepository;
        private readonly ISignerRepository _signerRepository;
        private readonly ICartableRepository _cartableRepository;
        private readonly ISettingsRepository _settingsRepository;
        public CommissionService(IUnitOfWork unitOfWork, IStudentRepository studentRepository,
            IProfileRepository profileRepository, IRequestRepository requestRepository,
            IMemberMasterRepository memberMasterRepository, IPersonRepository personRepository,
            ICommissionSpecialEducationRepository commissionSpecialEducationRepository,
            ICommissionRepository commissionRepository, IPostPersonRepository postPersonRepository,
            ISignerRepository signerRepository, ICartableRepository cartableRepository, ISettingsRepository settingsRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
            _profileRepository = profileRepository;
            _requestRepository = requestRepository;
            _memberMasterRepository = memberMasterRepository;
            _personRepository = personRepository;
            _commissionSpecialEducationRepository = commissionSpecialEducationRepository;
            _commissionRepository = commissionRepository;
            _postPersonRepository = postPersonRepository;
            _signerRepository = signerRepository;
            _cartableRepository = cartableRepository;
            _settingsRepository = settingsRepository;
        }

        /// <summary>
        /// ذخیره درخواست کمیسیون یا شورا به همراه پروفایل و دانشجو و شخص
        /// </summary>
        /// <param name="commissionModel"></param>
        /// <returns></returns>
        public Tuple<bool, string, long> AddCommissionRequest(CommissionModel commissionModel, long universityId)
        {
            try
            {
                //واکشی درخواست اگر وجود دارد
                var request = _requestRepository.Where(i => i.Id == commissionModel.Id).Include(i => i.Commission)
                    .Include(i => i.MemberMaster).FirstOrDefault() ?? new Request();
                request.RequestType = commissionModel.RequestType;
                request.NumberofRemainingUnits = commissionModel.NumberofRemainingUnits;
                request.NumberofSpentUnits = commissionModel.NumberofSpentUnits;
                request.RequestStatus = commissionModel.RequestStatus;
                request.Description = commissionModel.Description;
                request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Comision)
                    .OrderByDescending(p => p.Id).FirstOrDefault();

                var x = new PersianCalendar();
                commissionModel.Date =
                    x.ToDateTime(commissionModel.Date.Year, commissionModel.Date.Month, commissionModel.Date.Day, 0, 0, 0, 0, 0);
                //درج درخواست به همراه کمیسیون
                if (request.Id > 0)
                {
                    request.Commission.CommissionNumber = commissionModel.CommissionNumber;
                    request.Commission.Date = commissionModel.Date;
                    request.Commission.Description = commissionModel.Description;
                    _requestRepository.Update(request);
                }
                else
                {
                    request.PersonId = commissionModel.PersonId;
                    request.Commission = new Commission
                    {
                        CommissionNumber = commissionModel.CommissionNumber,
                        Date = commissionModel.Date,
                        Description = commissionModel.Description
                    };
                    _requestRepository.Add(request);
                }
                //آپدیت موارد خاص آموزشی مربوط به درخواست
                _commissionSpecialEducationRepository.Delete(c => c.CommissionId == commissionModel.Id);
                commissionModel.CommissionSpecialEducations.ForEach(c => c.CommissionId = commissionModel.Id);
                _commissionSpecialEducationRepository.Add(commissionModel.CommissionSpecialEducations);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, long>(true, "عملیات ثبت به درستی انجام شد", request.Id);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, long>(false, "خطا در ثبت درخواست", 0);
            }
        }
        //public Tuple<bool, string, long> AddCommissionRequestOld(CommissionModel commissionModel, long universityId)
        //{
        //    try
        //    {
        //        //واکشی دانشجو اگر وجود دارد وگرنه ایجاد می شود
        //        var student =
        //            _studentRepository.Where(s => s.StudentNumber == commissionModel.StudentNumber)
        //                .Include(i => i.Person).Include(i => i.Person.Profile).FirstOrDefault() ??
        //            new Student();
        //        student.StudentNumber = commissionModel.StudentNumber;
        //        student.FieldofStudyId = commissionModel.FieldofStudyId;
        //        student.Grade = commissionModel.Grade;
        //        student.MilitaryServiceStatus = commissionModel.MilitaryServiceStatus;

        //        //واکشی پروفایل اگر وجود دارد
        //        Profile profile;
        //        if (student.Person == null)
        //        {
        //            profile =
        //             _profileRepository.Where(p => p.NationalCode == commissionModel.NationalCode).FirstOrDefault() ??
        //             new Profile();
        //        }
        //        else
        //        {
        //            profile = student.Person.Profile;
        //        }
        //        profile.Name = commissionModel.Name;
        //        profile.Family = commissionModel.Family;
        //        profile.NationalCode = commissionModel.NationalCode;
        //        profile.Gender = commissionModel.Gender;

        //        //واکشی درخواست اگر وجود دارد
        //        var request = _requestRepository.Where(i => i.Id == commissionModel.Id).Include(i => i.Commission)
        //            .Include(i => i.MemberMaster).FirstOrDefault() ?? new Request();
        //        request.RequestType = commissionModel.RequestType;
        //        request.NumberofRemainingUnits = commissionModel.NumberofRemainingUnits;
        //        request.NumberofSpentUnits = commissionModel.NumberofSpentUnits;
        //        request.RequestStatus = commissionModel.RequestStatus;
        //        request.Description = commissionModel.Description;
        //        request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Comision)
        //            .OrderByDescending(p => p.Id).FirstOrDefault();

        //        long personId = 0;
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
        //        commissionModel.Date =
        //            x.ToDateTime(commissionModel.Date.Year, commissionModel.Date.Month, commissionModel.Date.Day, 0, 0, 0, 0, 0);
        //        //درج درخواست به همراه کمیسیون
        //        if (request.Id > 0)
        //        {
        //            request.Commission.CommissionNumber = commissionModel.CommissionNumber;
        //            request.Commission.Date = commissionModel.Date;
        //            request.Commission.Description = commissionModel.Description;
        //            _requestRepository.Update(request);
        //        }
        //        else
        //        {
        //            request.PersonId = personId;
        //            request.Commission = new Commission
        //            {
        //                CommissionNumber = commissionModel.CommissionNumber,
        //                Date = commissionModel.Date,
        //                Description = commissionModel.Description
        //            };
        //            _requestRepository.Add(request);
        //        }
        //        //آپدیت موارد خاص آموزشی مربوط به درخواست
        //        _commissionSpecialEducationRepository.Delete(c => c.CommissionId == commissionModel.Id);
        //        commissionModel.CommissionSpecialEducations.ForEach(c => c.CommissionId = commissionModel.Id);
        //        _commissionSpecialEducationRepository.Add(commissionModel.CommissionSpecialEducations);
        //        _unitOfWork.SaveChanges();
        //        return new Tuple<bool, string, long>(true, "عملیات ثبت به درستی انجام شد", request.Id);
        //    }
        //    catch (Exception exception)
        //    {
        //        return new Tuple<bool, string, long>(false, "خطا در ثبت درخواست", 0);
        //    }
        //}

        /// <summary>
        /// ذخیره درخواست کمیسیون یا شورا به همراه پروفایل و دانشجو و شخص
        /// </summary>
        /// <param name="commissionModel"></param>
        /// <returns></returns>
        public Tuple<bool, string, long> UpdateCommissionRequest(CommissionModel commissionModel)
        {
            try
            {
                var request = _requestRepository.Where(i => i.Id == commissionModel.Id).Include(i => i.Commission)
                    .Include(i => i.MemberMaster).FirstOrDefault();
                request.NumberofRemainingUnits = commissionModel.NumberofRemainingUnits;
                request.NumberofSpentUnits = commissionModel.NumberofSpentUnits;
                request.Description = commissionModel.Description;
                request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Comision).OrderByDescending(p => p.Id).FirstOrDefault();

                var x = new PersianCalendar();
                commissionModel.Date =
                    x.ToDateTime(commissionModel.Date.Year, commissionModel.Date.Month, commissionModel.Date.Day, 0, 0, 0, 0, 0);

                request.Commission.CommissionNumber = commissionModel.CommissionNumber;
                request.Commission.Date = commissionModel.Date;
                request.Commission.Description = commissionModel.Description;
                _requestRepository.Update(request);

                //آپدیت موارد خاص آموزشی مربوط به درخواست
                _commissionSpecialEducationRepository.Delete(c => c.CommissionId == commissionModel.Id);
                commissionModel.CommissionSpecialEducations.ForEach(c => c.CommissionId = commissionModel.Id);
                _commissionSpecialEducationRepository.Add(commissionModel.CommissionSpecialEducations);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, long>(true, "عملیات ثبت به درستی انجام شد", request.Id);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, long>(false, "خطا در ثبت درخواست", 0);
            }
        }
        //public Tuple<bool, string, long> UpdateCommissionRequestOld(CommissionModel commissionModel)
        //{
        //    try
        //    {
        //        var person =
        //            _personRepository.Where(s => s.Id == commissionModel.PersonId).Include(i => i.Profile)
        //                .Include(i => i.Student).FirstOrDefault();
        //        person.Student.StudentNumber = commissionModel.StudentNumber;
        //        person.Student.FieldofStudyId = commissionModel.FieldofStudyId;
        //        person.Student.Grade = commissionModel.Grade;
        //        person.Student.MilitaryServiceStatus = commissionModel.MilitaryServiceStatus;

        //        person.Profile.Name = commissionModel.Name;
        //        person.Profile.Family = commissionModel.Family;
        //        person.Profile.NationalCode = commissionModel.NationalCode;
        //        person.Profile.Gender = commissionModel.Gender;

        //        _studentRepository.Update(person.Student);
        //        _profileRepository.Update(person.Profile);

        //        var request = _requestRepository.Where(i => i.Id == commissionModel.Id).Include(i => i.Commission)
        //            .Include(i => i.MemberMaster).FirstOrDefault();
        //        request.NumberofRemainingUnits = commissionModel.NumberofRemainingUnits;
        //        request.NumberofSpentUnits = commissionModel.NumberofSpentUnits;
        //        request.Description = commissionModel.Description;
        //        request.MemberMaster = _memberMasterRepository.Where(d => d.RequestType == RequestType.Comision).OrderByDescending(p => p.Id).FirstOrDefault();

        //        var x = new PersianCalendar();
        //        commissionModel.Date =
        //            x.ToDateTime(commissionModel.Date.Year, commissionModel.Date.Month, commissionModel.Date.Day, 0, 0, 0, 0, 0);

        //        request.Commission.CommissionNumber = commissionModel.CommissionNumber;
        //        request.Commission.Date = commissionModel.Date;
        //        request.Commission.Description = commissionModel.Description;
        //        _requestRepository.Update(request);

        //        //آپدیت موارد خاص آموزشی مربوط به درخواست
        //        _commissionSpecialEducationRepository.Delete(c => c.CommissionId == commissionModel.Id);
        //        commissionModel.CommissionSpecialEducations.ForEach(c => c.CommissionId = commissionModel.Id);
        //        _commissionSpecialEducationRepository.Add(commissionModel.CommissionSpecialEducations);
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
        /// <param name="commissionId"></param>
        /// <returns></returns>
        public Tuple<bool, string, CommissionModel> FindInfoCommission(long commissionId)
        {
            try
            {
                var commissionModel = (from p in _requestRepository.Where(c => c.Id == commissionId && c.RequestType == RequestType.Comision)
                                           // .Include(i => i.Commission).Include(i => i.Person).Include(i => i.Person.Student).Include(i => i.Person.Profile)
                                       select new CommissionModel
                                       {
                                           Id = commissionId,
                                           NumberofRemainingUnits = p.NumberofRemainingUnits,
                                           NumberofSpentUnits = p.NumberofSpentUnits,
                                           RequestType = p.RequestType,
                                           RequestStatus = p.RequestStatus,

                                           CommissionNumber = p.Commission.CommissionNumber,
                                           Date = p.Commission.Date,
                                           Description = p.Commission.Description,

                                           PersonId = p.PersonId,
                                           NameFamily = p.Person.Profile.Name,
                                           NationalCode = p.Person.Profile.NationalCode,

                                           StudentNumber = p.Person.Student.StudentNumber,
                                           FieldofStudy = p.Person.Student.FieldofStudy.OrganizationStructureName.Name,
                                           Gender = p.Person.Profile.Gender,
                                           Grade = p.Person.Student.Grade,
                                           MilitaryServiceStatus = p.Person.Student.MilitaryServiceStatus
                                           //NumberofVotesCouncil = p.Requests.Count(r => r.Council != null)
                                       }).FirstOrDefault();
                if (commissionModel == null)
                    return new Tuple<bool, string, CommissionModel>(false, "رکورد کمیسیون نظر یافت نشد!", null);
                commissionModel.GenderString = commissionModel.Gender.GetDescription();
                commissionModel.GradeString = commissionModel.Grade.GetDescription();
                commissionModel.MilitaryServiceStatusString = commissionModel.MilitaryServiceStatus.GetDescription();

                return new Tuple<bool, string, CommissionModel>(true, "", commissionModel);
            }
            catch (Exception e)
            {
                return new Tuple<bool, string, CommissionModel>(false, "خطا در انجام عملیات!", null);
            }
        }

        /// <summary>
        /// شماره کمیسیون ارسال می شود
        /// </summary>
        /// <param name="universityId">ایدی دانشگاه</param>
        /// <returns>اگر متد با خطا مواجه شود شماره -1 ارسال می شود</returns>
        public Tuple<long, string> GetCommissionNumber(long universityId)
        {
            // شماره کمیسیون در صدور رای اورده شده است
            // یعنی اخرین شماره بر اساس تعداد کمیسیون هایی که برای انها رای صادر شده است بدست می آید
            try
            {
                var settingsUniversity = _settingsRepository.Where(s => s.UniversityId == universityId, sel => new Settings { CommissionNumberRepetitions = sel.CommissionNumberRepetitions }).FirstOrDefault();
                //if (settingsUniversity == null)
                //    return new Tuple<long, string>(-1,
                //        "رکورد تنظیمات در جدول وجود نداشت و به طور پیش فرض شماره کمیسیون -1 در نظر گرفته شده است");

                // اخرین کمیسیونی که برای آن رای صادر شده است
                var lastCommission = _commissionRepository.Where(s => s.Request.Person.UniversityId == universityId && s.Request.Vote != null, sel => new Commission { RequestId = sel.RequestId, CommissionNumber = sel.CommissionNumber }).OrderByDescending(d => d.RequestId).FirstOrDefault();
                if (lastCommission == null)// اگرکمیسیونی ذخیره نشده است
                    return new Tuple<long, string>(1, "کمیسیون ذخیره نشده است و شماره  کمیسیون از 1 شروع خواهد شد");

                // تعداد کمیسیون هایی که برای انها رای صادر شده
                var countInsertRequestVote = _commissionRepository.Where(s => s.Request.Person.UniversityId == universityId && s.Request.Vote != null && s.CommissionNumber == lastCommission.CommissionNumber).Count();
                if (settingsUniversity == null || countInsertRequestVote >= settingsUniversity.CommissionNumberRepetitions)
                    return new Tuple<long, string>(lastCommission.CommissionNumber + 1, "شماره کمیسیون بیشتر از حد مجاز شده است و یک شماره جدید اختصاص داده شده است");
                return new Tuple<long, string>(lastCommission.CommissionNumber, "شماره کمیسیون هنوز به مجاز نرسیده است");

            }
            catch (Exception ex)
            {
                return new Tuple<long, string>(-1, "خطا در انجام عملیات!");
            }

        }

        public Tuple<bool, string> IsValidCommissionNumber(long universityId, long commissionNumber)
        {
            try
            {
                var settingsUniversity = _settingsRepository.Where(s => s.UniversityId == universityId, sel => new Settings { CommissionNumberRepetitions = sel.CommissionNumberRepetitions }).FirstOrDefault();
                int countInsertrequest = _commissionRepository.Where(s => s.Request.Person.UniversityId == universityId && s.CommissionNumber == commissionNumber).Count();

                if (settingsUniversity != null)// اگر تنظیمات در جدول وجود داشت
                {
                    if (countInsertrequest < settingsUniversity.CommissionNumberRepetitions)// اگر هنوز به تعداد تکرار مورد نظر نرسیده ایم                    
                        return new Tuple<bool, string>(true, "شماره کمیسیون معتبر می باشد");
                    return new Tuple<bool, string>(false, "شماره کمیسیون بیشتر از حد مجاز می باشد ");
                }
                return new Tuple<bool, string>(false, "لطفا تنظیمات را بررسی نمایید ");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات!");
            }

        }
        public Tuple<bool, string, List<Commission>> GetCommissionsProfileStudent(string searcvalue, SearchType searchtype)
        {
            try
            {
                Int64 number = (searchtype == SearchType.FileNumber || searchtype == SearchType.StudentNumber)
                   ? Convert.ToInt64(searcvalue) : 0;

                var query =
                    _commissionRepository.Where(
                        s =>
                            (searchtype == SearchType.StudentNumber)
                                ? s.Request.Person.Student.StudentNumber == number
                                : (searchtype == SearchType.StudentNameFamili)
                                    ? (s.Request.Person.Profile.Name + " " + s.Request.Person.Profile.Family).Contains(searcvalue)

                                    : (searchtype == SearchType.FileNumber) && s.CommissionNumber == number);


                var commissions = query.Include(i => i.Request.Vote).Include(i => i.Request.Person.Profile).Include(i => i.Request.Person.Student)
                    .Include(i => i.CommissionSpecialEducations.Select(s => s.SpecialEducation)).ToList();
                return new Tuple<bool, string, List<Commission>>(true, "", commissions);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, List<Commission>>(false, "خطا در انجام عملیات!", null);
            }
        }
    }
}
