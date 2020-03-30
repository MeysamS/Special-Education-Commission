using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Microsoft.AspNet.Identity;

namespace Comision.Service.ImplementationService
{
    public class PersonManagementService : IPersonManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfileRepository _profileRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IPersonelRepository _personelRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IPostPersonRepository _postPersonRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserPostRepository _userPostRepository;
        private readonly IApplicationUserManager _userManager;

        public PersonManagementService(IProfileRepository profileRepository, IStudentRepository studentRepository
            , IPersonelRepository personelRepository, IPersonRepository personRepository, IUnitOfWork unitOfWork,
            IPostPersonRepository postPersonRepository, IUserRoleRepository userRoleRepository,
            IUserRepository userRepository, IUserPostRepository userPostRepository, IApplicationUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _profileRepository = profileRepository;
            _studentRepository = studentRepository;
            _personelRepository = personelRepository;
            _personRepository = personRepository;
            _postPersonRepository = postPersonRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _userPostRepository = userPostRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// این متد یک مدل از پروفایل را دریافت و اگر وجود داشته باشد ویرایش در غیر این صورت درج می کند
        /// </summary>
        /// <param name="modelProfile">مدل پروفایل</param>
        /// <returns>وضعیت ذخیره شدن متد</returns>
        public Tuple<bool, string, Profile> AddOrUpdateProfile(ProfileModel modelProfile, long universityId)
        {
            try
            {
                var profile =
                    _profileRepository.Where(p => p.NationalCode == modelProfile.NationalCode).FirstOrDefault();
                if (profile != null && modelProfile.PersonId != profile.PersonId)
                    return new Tuple<bool, string, Profile>(false, "شماره ملی تکراری می باشد!", null);
                // از آنجا که این متد ممکن از توسط مدل در ج دانشجو در فرم درخواست کمیسیون یا شورا
                // فرا خوانی می شود ممکن است در انجا برخی از فیل ها پر نشود برای همین شرط نال بررسی می شود 
                var newProfile = _profileRepository.Find(s => s.PersonId == modelProfile.PersonId) ?? new Profile();

                newProfile.PersonId = modelProfile.PersonId;
                newProfile.Name = modelProfile.Name;
                newProfile.Family = modelProfile.Family;
                if (newProfile.Mobile != null)
                    newProfile.Mobile = modelProfile.Mobile;

                newProfile.Gender = modelProfile.Gender;

                if (newProfile.Avatar != null)
                    newProfile.Avatar = modelProfile.Avatar;

                newProfile.EmailUnivercity = modelProfile.EmailUnivercity;

                if (newProfile.AddressWork != null)
                    newProfile.AddressWork = modelProfile.AddressWork;

                if (newProfile.PhoneWork != null)
                    newProfile.PhoneWork = modelProfile.PhoneWork;
                if (newProfile.WebSite != null)
                    newProfile.WebSite = modelProfile.WebSite;

                if (newProfile.Description != null)
                    newProfile.Description = modelProfile.Description;
                newProfile.NationalCode = modelProfile.NationalCode;
                if (newProfile.BirthcertificateNumber != null)
                    newProfile.BirthcertificateNumber = modelProfile.BirthcertificateNumber;

                if (newProfile.BrithDate != null)
                    newProfile.BrithDate = modelProfile.BrithDate?.ToMiladiDate();

                if (newProfile.PersonId <= 0)
                {
                    var person = new Person
                    {
                        Active = true,
                        UniversityId = universityId,
                        Profile = newProfile
                    };
                    _personRepository.Add(person);
                    newProfile.PersonId = person.Id;
                }
                _profileRepository.AddOrUpdate(p => p.PersonId, newProfile);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, Profile>(true, "عملیات ثبت پروفایل انجام شد", newProfile);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, Profile>(false, "خطا در ثبت علیات پروفایل", null);
            }
        }

        public Tuple<bool, string> UpdateProfile(Profile model)
        {
            try
            {
                _profileRepository.Update(model);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ویرایش انجام شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در ویرایش اطلاعات");
            }
        }

        public Tuple<bool, string, Personel> AddOrUpdatePersonel(PersonelModel personelModel, long universityId)
        {
            try
            {
                var personel = _personelRepository.Find(p => p.PersonId == personelModel.PersonId) ?? new Personel();

                personel.PersonId = personelModel.PersonId;
                personel.DateOfEmployeement = personelModel.DateOfEmployeement?.ToMiladiDate();
                personel.EmployeeNumber = personelModel.EmployeeNumber;
                personel.Grade = personelModel.Grade;
                personel.Active = personelModel.Active;
                personel.Signature = personelModel.Signature;

                if (personel.PersonId <= 0)
                {
                    var person = new Person
                    {
                        Active = true,
                        UniversityId = universityId,
                        Personel = personel
                    };
                    _personRepository.Add(person);
                    personel.PersonId = person.Id;
                }
                _personelRepository.AddOrUpdate(p => p.PersonId, personel);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, Personel>(true, "عملیات ثبت پرسنل انجام شد", personel);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, Personel>(false, "خطا در ثبت علیات پرسنل", null);
            }
        }
        public Tuple<bool, string> UpdatePersonel(Personel model)
        {
            try
            {
                _personelRepository.Update(model);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ویرایش انجام شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در ویرایش اطلاعات");
            }
        }

        public Tuple<bool, string, Student> AddOrUpdateStudent(StudentModel studentModel, long universityId)
        {
            try
            {
                var student = _studentRepository.Find(s => s.PersonId == studentModel.PersonId) ?? new Student();

                student.PersonId = studentModel.PersonId;
                student.Grade = studentModel.Grade;
                student.FieldofStudyId = studentModel.FieldofStudyId;
                student.MilitaryServiceStatus = studentModel.MilitaryServiceStatus;
                student.StudentNumber = studentModel.StudentNumber;
                student.Active = studentModel.Active;

                if (student.PersonId <= 0)
                {
                    var person = new Person
                    {
                        Active = true,
                        UniversityId = universityId,
                        Student = student
                    };
                    _personRepository.Add(person);
                    student.PersonId = person.Id;
                }
                _studentRepository.AddOrUpdate(p => p.PersonId, student);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, Student>(true, "عملیات ثبت دانشجو انجام شد", student);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, Student>(false, "خطا در ثبت علیات دانشجو", null);
            }
        }
        public Tuple<bool, string> UpdateStudent(Student model)
        {
            try
            {
                _studentRepository.Update(model);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ویرایش انجام شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در ویرایش اطلاعات");
            }
        }

        public Tuple<bool, string> AddOrUpdateStudentProfile(StudentProfileModel studentProfileModel, long universityId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var stateProfile = AddOrUpdateProfile(new ProfileModel { PersonId = studentProfileModel.PersonId, Name = studentProfileModel.Name, Family = studentProfileModel.Family, NationalCode = studentProfileModel.NationalCode, Gender = studentProfileModel.Gender }, universityId);
                if (stateProfile.Item1 == false)
                {
                    _unitOfWork.Rollback();
                    return new Tuple<bool, string>(false, stateProfile.Item2);
                }
                var stateStudent = AddOrUpdateStudent(new StudentModel { PersonId = stateProfile.Item3.PersonId, Grade = studentProfileModel.Grade, FieldofStudyId = studentProfileModel.FieldofStudyId, MilitaryServiceStatus = studentProfileModel.MilitaryServiceStatus, StudentNumber = studentProfileModel.StudentNumber }, universityId);
                if (stateStudent.Item1 == false)
                {
                    _unitOfWork.Rollback();
                    return new Tuple<bool, string>(false, stateStudent.Item2);
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
                return new Tuple<bool, string>(true, "عملیات ثبت دانشجو انجام شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت عملیات دانشجو");
            }
        }

        public Tuple<bool, string, User> AddOrUpdateUser(UserModel userModel, long universityId)
        {
            try
            {
                var user = _userRepository.Find(u => u.Id == userModel.PersonId) ?? new User();

                user.Id = userModel.PersonId;
                user.Email = userModel.Email;
                user.UserName = userModel.UserName;
                user.EmailConfirmed = userModel.EmailConfirmed;
                user.LockoutEnabled = !userModel.Active;

                if (user.Id <= 0)
                {
                    var person = new Person
                    {
                        Active = true,
                        UniversityId = universityId,
                        User = user
                    };
                    _personRepository.Add(person);
                    user.Id = person.Id;
                }
                _userRepository.AddOrUpdate(p => p.Id, user);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, User>(true, "عملیات ثبت کاربر انجام شد", user);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, User>(false, "خطا در ثبت علیات کاربری", null);
            }
        }
        public Tuple<bool, string> UpdateUser(User model)
        {
            try
            {
                _userRepository.Update(model);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ویرایش انجام شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در ویرایش اطلاعات");
            }
        }

        public Tuple<bool, string> AddOrUpdatePassword(PasswordModel passwordModel)
        {
            try
            {
                if (passwordModel.NewPasswordHash == passwordModel.RepeatPasswordHash)
                {
                    var user = _userRepository.Find(u => u.Id == passwordModel.PersonId);
                    if (user == null)
                        return new Tuple<bool, string>(false, "کاربر مورد نظر وجود ندارد - خطا در ثبت عملیات");
                    else
                    {
                        var isvalidpassword =
                            _userManager.CheckPasswordAsync(user, passwordModel.OldPasswordHash).Result;

                        if (isvalidpassword)
                        {
                            var newPasswordHash = _userManager.PasswordHasher.HashPassword(passwordModel.NewPasswordHash);
                            user.PasswordHash = newPasswordHash;
                            _userManager.UpdateAsync(user);
                            _unitOfWork.SaveChanges();


                            //_userManager.ChangePasswordAsync(passwordModel.PersonId, passwordModel.OldPasswordHash,passwordModel.NewPasswordHash);
                            //_unitOfWork.SaveChanges();
                            return new Tuple<bool, string>(true, "عملیات تغییر کلمه عبور انجام شد");
                        }
                        else
                            return new Tuple<bool, string>(false, "کلمه عبور قبلی اشتباه می باشد- خطا در ثبت عملیات");
                    }
                }
                else
                    return new Tuple<bool, string>(false, "کلمه عبور جدید باید با تکرارکلمه عبور جدید برابر باشد - خطا در ثبت عملیات");

            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت علیات تغییر کلمه عبور");
            }
        }

        /// <summary>
        /// ایت متد یک مدل از پرسنل و یک مدل از پروفایل دریافت می کند و ویرایش یا درج می کند
        /// اگر وجود نداشته باشد درج و اگر قبلا رکوردی از پروفایل یا اطلاعات پرسنلی وجود داشته باشد ویرایش می کند
        /// </summary>
        /// <param name="personelProfileModel">مدل پرسنلی و اطلاعات پروفایلی</param>
        /// <returns></returns>
        public Tuple<bool, string> AddOrUpdatePersonelProfile(PersonelProfileModel personelProfileModel, long universityId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (_personRepository.Contains(p => p.Id == personelProfileModel.ModelProfile.PersonId) == false)
                    // چون ایدی پروفایل همش مقدار دارد
                    return new Tuple<bool, string>(false, "خطا در انجام عملیات - شخص مورد نظر برای ویرایش وجود ندارد");
                // ممکن است ایدی پرسنا مقدار نداشته باشد چون هنوز پرسنلی به شخص نسبت داده نشده است پس باید یک ایدی بگیرد
                personelProfileModel.PersonId = personelProfileModel.ModelProfile.PersonId;

                var objPersonel = _personelRepository.Find(p => p.PersonId == personelProfileModel.PersonId) ??
                                  new Personel();

                objPersonel.PersonId = personelProfileModel.ModelProfile.PersonId;
                objPersonel.Grade = personelProfileModel.Grade;
                objPersonel.EmployeeNumber = personelProfileModel.EmployeeNumber;
                objPersonel.DateOfEmployeement = personelProfileModel.DateOfEmployeement?.ToMiladiDate();

                if (objPersonel.PersonId <= 0)
                {
                    var person = new Person
                    {
                        Active = true,
                        UniversityId = universityId,
                        Personel = objPersonel
                    };
                    _personRepository.Add(person);
                    objPersonel.PersonId = person.Id;
                }
                _personelRepository.AddOrUpdate(p => p.PersonId, objPersonel);
                _unitOfWork.SaveChanges();

                personelProfileModel.PersonId = objPersonel.PersonId;
                var state = AddOrUpdateProfile(personelProfileModel.ModelProfile, universityId);
                if (state.Item1 == false)
                {
                    _unitOfWork.Rollback();
                    return new Tuple<bool, string>(false, "خطا در ثبت علیات ");
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                return new Tuple<bool, string>(false, "خطا در ثبت علیات ");
            }
        }

        /// <summary>
        /// ایت متد یک مدل از پرسنل ، دانشجو ، پروفایل و کاربر دریافت می کند و ویرایش یا درج می کند
        /// اگر وجود نداشته باشد درج و اگر قبلا رکوردی از آنها وجود داشته باشد ویرایش می کند
        /// </summary>
        /// <param name="profileFullInfoModel">مدل پرسنلی و اطلاعات پروفایلی</param>
        /// <returns></returns>
        public Tuple<bool, string> AddOrUpdateProfileFullInfo(ProfileFullInfoModel profileFullInfoModel, long universityId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (_personRepository.Contains(p => p.Id == profileFullInfoModel.ProfileModel.PersonId) == false)
                    // چون ایدی پروفایل همش مقدار دارد
                    return new Tuple<bool, string>(false, "خطا در انجام عملیات - شخص مورد نظر برای ویرایش وجود ندارد");
                var stateProfile = AddOrUpdateProfile(profileFullInfoModel.ProfileModel, universityId);
                if (stateProfile.Item1 == false)
                {
                    _unitOfWork.Rollback();
                    return new Tuple<bool, string>(false, stateProfile.Item2);
                }
                profileFullInfoModel.PersonelModel.PersonId = stateProfile.Item3.PersonId;
                var statePersonel = AddOrUpdatePersonel(profileFullInfoModel.PersonelModel, universityId);
                if (statePersonel.Item1 == false)
                {
                    _unitOfWork.Rollback();
                    return new Tuple<bool, string>(false, statePersonel.Item2);
                }
                profileFullInfoModel.StudentModel.PersonId = statePersonel.Item3.PersonId;
                var stateStudent = AddOrUpdateStudent(profileFullInfoModel.StudentModel, universityId);
                if (stateStudent.Item1 == false)
                {
                    _unitOfWork.Rollback();
                    return new Tuple<bool, string>(false, stateStudent.Item2);
                }

                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                return new Tuple<bool, string>(false, "خطا در ثبت علیات پروفایلی");
            }
        }
        public Tuple<bool, string> AddOrUpdateAssignmentPost(PostPersonModel postPersonModel)
        {
            try
            {
                var validationAssignment = true;
                var postPerson = new PostPerson
                {
                    PostId = postPersonModel.PostId,
                    PersonId = postPersonModel.PersonId
                };
                switch (postPersonModel.PostType)
                {
                    case PostType.CentralOrganization:
                        if (
                            _postPersonRepository.Contains(
                                p => p.PostId == postPerson.PostId && p.CentralOrganizationId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        postPerson.CentralOrganizationId = postPersonModel.LevelId;
                        break;
                    case PostType.BranchProvince:
                        if (
                            _postPersonRepository.Contains(
                                p => p.PostId == postPerson.PostId && p.BranchProvinceId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        postPerson.BranchProvinceId = postPersonModel.LevelId;
                        break;
                    case PostType.University:
                        if (
                            _postPersonRepository.Contains(
                                p => p.PostId == postPerson.PostId && p.UniversityId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        postPerson.UniversityId = postPersonModel.LevelId;
                        break;
                    case PostType.College:
                        if (
                            _postPersonRepository.Contains(
                                p => p.PostId == postPerson.PostId && p.CollegeId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        postPerson.CollegeId = postPersonModel.LevelId;
                        break;
                    case PostType.EducationalGroup:
                        if (
                            _postPersonRepository.Contains(
                                p => p.PostId == postPerson.PostId && p.EducationalGroupId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        postPerson.EducationalGroupId = postPersonModel.LevelId;
                        break;
                    case PostType.FieldofStudy:
                        if (
                            _postPersonRepository.Contains(
                                p => p.PostId == postPerson.PostId && p.FieldofStudyId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        postPerson.FieldofStudyId = postPersonModel.LevelId;
                        break;
                }
                if (!validationAssignment)
                    return new Tuple<bool, string>(false, "این سمت قبلا به شخص دیگری انتساب داده شده است!");

                _postPersonRepository.AddOrUpdate(postPerson);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت علیات ");
            }
        }

        public Tuple<bool, string> AddOrUpdateAssignmentPostUser(PostPersonModel postPersonModel)
        {
            try
            {
                var validationAssignment = true;
                var userpost = new UserPost
                {
                    PostId = postPersonModel.PostId,
                    UserId = postPersonModel.PersonId
                };
                switch (postPersonModel.PostType)
                {
                    case PostType.CentralOrganization:
                        if (
                            _userPostRepository.Contains(
                                p => p.PostId == userpost.PostId && p.UserId == userpost.UserId && p.CentralOrganizationId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        userpost.CentralOrganizationId = postPersonModel.LevelId;
                        break;
                    case PostType.BranchProvince:
                        if (
                            _userPostRepository.Contains(
                                p => p.PostId == userpost.PostId && p.UserId == userpost.UserId && p.BranchProvinceId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        userpost.BranchProvinceId = postPersonModel.LevelId;
                        break;
                    case PostType.University:
                        if (
                            _userPostRepository.Contains(
                                p => p.PostId == userpost.PostId && p.UserId == userpost.UserId && p.UniversityId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        userpost.UniversityId = postPersonModel.LevelId;
                        break;
                    case PostType.College:
                        if (
                            _userPostRepository.Contains(
                                p => p.PostId == userpost.PostId && p.UserId == userpost.UserId && p.CollegeId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        userpost.CollegeId = postPersonModel.LevelId;
                        break;
                    case PostType.EducationalGroup:
                        if (
                            _userPostRepository.Contains(
                                p => p.PostId == userpost.PostId && p.UserId == userpost.UserId && p.EducationalGroupId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        userpost.EducationalGroupId = postPersonModel.LevelId;
                        break;
                    case PostType.FieldofStudy:
                        if (
                            _userPostRepository.Contains(
                                p => p.PostId == userpost.PostId && p.UserId == userpost.UserId && p.FieldofStudyId == postPersonModel.LevelId))
                        {
                            validationAssignment = false;
                            break;
                        }
                        userpost.FieldofStudyId = postPersonModel.LevelId;
                        break;
                }
                if (!validationAssignment)
                    return new Tuple<bool, string>(false, "این اختیار قبلا به کاربر مورد نظر داده شده است و تکراری می باشد");

                _userPostRepository.AddOrUpdate(userpost);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت عملیات ");
            }
        }

        public Tuple<bool, string> AddOrUpdateAssignmentRole(UserRoleModel userRoleModel)
        {
            try
            {
                var userRole = new UserRole
                {
                    RoleId = userRoleModel.RoleId,
                    UserId = userRoleModel.UserId
                };
                _userRoleRepository.AddOrUpdate(p => new { p.UserId, p.RoleId }, userRole);

                ////if (userRoleModel.PostId != null)
                ////{
                ////    var userPost = new UserPost
                ////    {
                ////        PostId = (long) userRoleModel.PostId,
                ////        UserId = userRoleModel.UserId
                ////    };
                ////    _userPostRepository.AddOrUpdate(p => new {p.UserId, p.PostId}, userPost);
                ////}
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت علیات ");
            }
        }
        public Tuple<bool, string, string> DeletePersonel(long personId)
        {
            try
            {
                if (_personelRepository.Contains(x => x.PersonId == personId && x.Person.Cartables.Count > 0))
                    return new Tuple<bool, string, string>(false, "پرسنل در سیستم فعالیت داشته ، لذا قابل حذف نمی باشد", "");
                var data = _personelRepository.Find(x => x.PersonId == personId);
                var signatue = "";
                if (data == null)
                    return new Tuple<bool, string, string>(false, "پرسنل مورد نظر یافت نشد!", "");
                signatue = data.Signature;
                _personelRepository.Delete(data);
                _postPersonRepository.Delete(p => p.PersonId == personId);

                _unitOfWork.SaveChanges();
                return new Tuple<bool, string, string>(true, "عملیات حذف انجام شد", signatue);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, string>(false, "خطا در انجام عملیات", "");
            }
        }
        public Tuple<bool, string> DeleteStudent(long personId)
        {
            try
            {
                var data = _studentRepository.Contains(x => x.PersonId == personId && x.Person.Requests.Count > 0);
                if (data)
                    return new Tuple<bool, string>(false, "دانشجو در سیستم فعالیت داشته ، لذا قابل حذف نمی باشد");
                _studentRepository.Delete(x => x.PersonId == personId);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> DeleteAssignmentPost(long postId, long personId, PostType postType, long levelId)
        {
            try
            {
                switch (postType)
                {
                    case PostType.CentralOrganization:
                        _postPersonRepository.Delete(x => x.PostId == postId && x.PersonId == personId && x.CentralOrganizationId == levelId);
                        break;
                    case PostType.BranchProvince:
                        _postPersonRepository.Delete(x => x.PostId == postId && x.PersonId == personId && x.BranchProvinceId == levelId);
                        break;
                    case PostType.University:
                        _postPersonRepository.Delete(x => x.PostId == postId && x.PersonId == personId && x.UniversityId == levelId);
                        break;
                    case PostType.College:
                        _postPersonRepository.Delete(x => x.PostId == postId && x.PersonId == personId && x.CollegeId == levelId);
                        break;
                    case PostType.EducationalGroup:
                        _postPersonRepository.Delete(x => x.PostId == postId && x.PersonId == personId && x.EducationalGroupId == levelId);
                        break;
                    case PostType.FieldofStudy:
                        _postPersonRepository.Delete(x => x.PostId == postId && x.PersonId == personId && x.FieldofStudyId == levelId);
                        break;
                }

                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        public Tuple<bool, string> DeleteAssignmentPostUser(long postId, long personId, PostType postType, long levelId)
        {
            try
            {
                switch (postType)
                {
                    case PostType.CentralOrganization:
                        _userPostRepository.Delete(x => x.PostId == postId && x.UserId == personId && x.CentralOrganizationId == levelId);
                        break;
                    case PostType.BranchProvince:
                        _userPostRepository.Delete(x => x.PostId == postId && x.UserId == personId && x.BranchProvinceId == levelId);
                        break;
                    case PostType.University:
                        _userPostRepository.Delete(x => x.PostId == postId && x.UserId == personId && x.UniversityId == levelId);
                        break;
                    case PostType.College:
                        _userPostRepository.Delete(x => x.PostId == postId && x.UserId == personId && x.CollegeId == levelId);
                        break;
                    case PostType.EducationalGroup:
                        _userPostRepository.Delete(x => x.PostId == postId && x.UserId == personId && x.EducationalGroupId == levelId);
                        break;
                    case PostType.FieldofStudy:
                        _userPostRepository.Delete(x => x.PostId == postId && x.UserId == personId && x.FieldofStudyId == levelId);
                        break;
                }

                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> DeleteAssignmentRole(long userId, long roleId)
        {
            try
            {
                _userRoleRepository.Delete(x => x.UserId == userId && x.RoleId == roleId);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public List<Person> GetUsers(Expression<Func<Person, bool>> orderByProperty,
            AuthenticationType userLoginAuthenticationType, UserType userType, long levelId, bool isAscendingOrder, out int pageCount, int pageNum = 1, int pageSize = 20)
        {
            try
            {

                var persons = _personRepository.Where(p =>
                       p.User.Authentication.AuthenticationType != AuthenticationType.AdminCentral
                       && p.User.Authentication.AuthenticationType != AuthenticationType.AdminBranch
                       && p.User.Authentication.AuthenticationType != AuthenticationType.AdminUniversity
                       && (userType == UserType.Student ? (p.Student != null) : (userType != UserType.Personel || p.Personel != null)))
                    .Include(s => s.User.Roles)
                    .Include(i => i.User.Roles.Select(t => t.Role))
                    .Include(s => s.Profile)
                    .Include(i => i.PostPersons)
                    .Include(i => i.User.UserPosts)
                    .Include(i => i.PostPersons.Select(t => t.Post));

                return _personRepository.PagedResult(persons, orderByProperty, isAscendingOrder, out pageCount, pageNum, pageSize).ToList();
            }
            catch (Exception)
            {
                pageCount = 0;
                return null;
            }
        }

        /// <summary>
        /// اطلاعات پرسنلی و پروفایلی یک شخص دریافت می شود
        /// </summary>
        /// <param name="personId">ایدی شخص</param>
        /// <returns></returns>
        public Tuple<bool, string, PersonelProfileModel> GetPersonelProfile(long personId)
        {
            try
            {
                PersonelProfileModel objPersonelProfileModel;
                var personProfile =
                    _personRepository.Where(p => p.Id == personId)
                        .Include(s => s.Personel)
                        .Include(s => s.Profile)
                        .FirstOrDefault();
                if (personProfile != null)
                {
                    objPersonelProfileModel = new PersonelProfileModel();
                    if (personProfile.Personel != null)
                    {
                        objPersonelProfileModel.PersonId = personProfile.Id;
                        if (personProfile.Personel.DateOfEmployeement != null)
                            objPersonelProfileModel.DateOfEmployeement = personProfile.Personel.DateOfEmployeement;

                        objPersonelProfileModel.EmployeeNumber = personProfile.Personel.EmployeeNumber;
                        objPersonelProfileModel.Grade = personProfile.Personel.Grade;
                    }
                    if (personProfile.Profile != null)
                        objPersonelProfileModel.ModelProfile = new ProfileModel(personProfile.Profile);
                }
                else
                    return new Tuple<bool, string, PersonelProfileModel>(false, "پرسنل مورد نظر پیدا نشده است", null);

                return new Tuple<bool, string, PersonelProfileModel>(true, "شخص مورد نظر پیدا شده  است",
                    objPersonelProfileModel);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, PersonelProfileModel>(false, "خطا در انجام عملیات", null);
            }
        }

        public Profile Find(long profileId)
        {
            return _profileRepository.Find(x => x.PersonId == profileId);
        }
        public Person FindPerson(long personId)
        {
            return _personRepository.Find(x => x.Id == personId);
        }
        public Profile FindProfile(long profileId)
        {
            return _profileRepository.Find(x => x.PersonId == profileId);
        }
        public Personel FindPersonel(long personelId)
        {
            return _personelRepository.Find(x => x.PersonId == personelId);
        }
        public Student FindStudent(long studentId)
        {
            return _studentRepository.Find(x => x.PersonId == studentId);
        }

        /// <summary>
        /// اطلاعات پرسنلی دانشجویی و پروفایلی و کاربری یک شخص دریافت می شود
        /// </summary>
        /// <param name="personId">ایدی شخص</param>
        /// <returns></returns>
        public Tuple<bool, string, ProfileFullInfoModel> GetProfileFullInfo(long personId)
        {
            try
            {
                var person =
                    _personRepository.Where(p => p.Id == personId)
                        .Include(s => s.Personel)
                        .Include(s => s.Student)
                        .Include(s => s.User)
                        .Include(s => s.Profile)
                        .FirstOrDefault();
                if (person == null)
                    return new Tuple<bool, string, ProfileFullInfoModel>(false, "پرسنل مورد نظر پیدا نشده است", null);
                var profileFullInfo = new ProfileFullInfoModel();
                //if (person.Personel != null)
                profileFullInfo.PersonelModel = new PersonelModel(person.Personel);
                //if (person.Student != null)
                profileFullInfo.StudentModel = new StudentModel(person.Student);
                //if (person.Profile != null)
                profileFullInfo.ProfileModel = new ProfileModel(person.Profile);
                //if (person.User != null)
                profileFullInfo.UserModel = new UserModel(person.User);

                profileFullInfo.PasswordModel = new PasswordModel(person.User.Id);

                return new Tuple<bool, string, ProfileFullInfoModel>(true, "شخص مورد نظر پیدا شده  است",
                    profileFullInfo);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, ProfileFullInfoModel>(false, "خطا در انجام عملیات", null);
            }
        }

        /// <summary>
        /// این متد فقط لیست پرسنل ها را بر می گرداند به همراه اطلاعات پروفایلشان
        /// </summary>
        /// <returns>لیسن از پرسنل و پروفایل مدل ابلته برخی از فیلد ها پر نمی شود</returns>
        public Tuple<bool, string, List<PersonelProfileModel>> GetPersonelProfiles(LevelProgram levelProgram,
            long leveLId)
        {
            try
            {
                var query =
                    _personelRepository.Where(
                        s =>
                            levelProgram == LevelProgram.CentralOrganization
                                ? s.Person.CentralOrganizationId == leveLId
                                : levelProgram == LevelProgram.BranchProvince
                                    ? s.Person.BranchProvinceId == leveLId
                                    : s.Person.UniversityId == leveLId).Include(s => s.Person)
                        .Include(s => s.Person.Profile);

                // لیست پرسنل به همراه اطلاعات شخص و پروفایل آن برای دانشگاه
                var listPersonelProfileModel =
                    from sel in query
                    select new PersonelProfileModel
                    {
                        PersonId = sel.PersonId,
                        DateOfEmployeement = sel.DateOfEmployeement,
                        EmployeeNumber = sel.EmployeeNumber,
                        Grade = sel.Grade,
                        ModelProfile = new ProfileModel
                        {
                            PersonId = sel.Person.Id,
                            Name = sel.Person.Profile.Name,
                            Family = sel.Person.Profile.Family,
                            Gender = sel.Person.Profile.Gender,
                            NationalCode = sel.Person.Profile.NationalCode
                        }
                    };
                return new Tuple<bool, string, List<PersonelProfileModel>>(true, "خطا در انجام عملیات", listPersonelProfileModel.ToList());
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<PersonelProfileModel>>(false, "خطا در انجام عملیات", null);
            }
        }
        public Tuple<bool, string, List<PostPersonModel>> GetPostofPersonel(long personId)
        {
            try
            {
                var query = _postPersonRepository.Where(w => w.PersonId == personId)
                        .Select(s => new
                        {
                            s.PostId,
                            s.PersonId,
                            s.Post.Name,
                            s.Post.PostType,
                            LevelId = (s.CentralOrganizationId ?? (s.BranchProvinceId ?? (s.UniversityId ?? (s.CollegeId ?? (s.EducationalGroupId ?? s.FieldofStudyId))))),
                            LevelName = (s.CentralOrganization != null ? s.CentralOrganization.Name
                                : (s.BranchProvince != null ? s.BranchProvince.Name : (s.University != null ? s.University.Name
                                : (s.College != null ? s.College.OrganizationStructureName.Name
                                : (s.EducationalGroup != null ? s.EducationalGroup.OrganizationStructureName.Name
                                : (s.FieldofStudy.OrganizationStructureName.Name))))))
                        }).AsEnumerable();
                var postPersonList = (from a in query
                                      select new PostPersonModel
                                      {
                                          PostId = a.PostId,
                                          PersonId = a.PersonId,
                                          PostName = a.Name,
                                          PostType = a.PostType,
                                          LevelId = Convert.ToInt64(a.LevelId),
                                          LevelName = a.LevelName
                                      }).ToList();
                return new Tuple<bool, string, List<PostPersonModel>>(true, "خطا در انجام عملیات", postPersonList);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<PostPersonModel>>(false, "خطا در انجام عملیات", null);
            }
        }

        public Tuple<bool, string, List<PostPersonModel>> GetPostofUser(long personId)
        {
            try
            {
                var query = _userPostRepository.Where(w => w.UserId == personId)
                        .Select(s => new
                        {
                            s.PostId,
                            s.UserId,
                            s.Post.Name,
                            s.Post.PostType,
                            LevelId = (s.CentralOrganizationId ?? (s.BranchProvinceId ?? (s.UniversityId ?? (s.CollegeId ?? (s.EducationalGroupId ?? s.FieldofStudyId))))),
                            LevelName = (s.CentralOrganization != null ? s.CentralOrganization.Name
                                : (s.BranchProvince != null ? s.BranchProvince.Name : (s.University != null ? s.University.Name
                                : (s.College != null ? s.College.OrganizationStructureName.Name
                                : (s.EducationalGroup != null ? s.EducationalGroup.OrganizationStructureName.Name
                                : (s.FieldofStudy.OrganizationStructureName.Name))))))
                        }).AsEnumerable();
                var postUserList = (from a in query
                                    select new PostPersonModel
                                    {
                                        PostId = a.PostId,
                                        PersonId = a.UserId,
                                        PostName = a.Name,
                                        PostType = a.PostType,
                                        LevelId = Convert.ToInt64(a.LevelId),
                                        LevelName = a.LevelName
                                    }).ToList();
                return new Tuple<bool, string, List<PostPersonModel>>(true, "خطا در انجام عملیات", postUserList);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<PostPersonModel>>(false, "خطا در انجام عملیات", null);
            }
        }

        /// <summary>
        /// لیست سمت ها و اختیارات به همراه امضاکنندگان مربوط به یک شخص
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Tuple<bool, string, List<PostPerson>> GetPostSignersofPersonel(long personId)
        {
            try
            {
                var postSingers = _postPersonRepository.Where(w => w.PersonId == personId).Include(i => i.Post)
                    .Include(i => i.Post.Signers).ToList();
                postSingers.AddRange(_userPostRepository.Where(p => p.UserId == personId)
                    .Include(i => i.Post.Signers).ToList()
                    .Select(s => new PostPerson
                    {
                        Post = s.Post,
                        BranchProvinceId = s.BranchProvinceId,
                        CentralOrganizationId = s.CentralOrganizationId,
                        CollegeId = s.CollegeId,
                        EducationalGroupId = s.EducationalGroupId,
                        PersonId = s.UserId,
                        PostId = s.PostId,
                        UniversityId = s.UniversityId,
                        FieldofStudyId = s.FieldofStudyId
                        //Person = s.User.Person

                    }).ToList());
                return new Tuple<bool, string, List<PostPerson>>(true, "عملیات با موفقیت انجام شد", postSingers);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<PostPerson>>(false, "خطا در انجام عملیات", null);
            }
        }
        public Tuple<bool, string, List<UserProfileModel>> GetUsers(LevelProgram levelProgram,
            long leveLId)
        {
            try
            {
                var userRoles = _userRepository.Where(s =>
                           levelProgram == LevelProgram.CentralOrganization
                               ? s.Person.CentralOrganizationId == leveLId
                               : levelProgram == LevelProgram.BranchProvince
                                   ? s.Person.BranchProvinceId == leveLId
                                   : s.Person.UniversityId == leveLId).Where(
                    w => w.EmailConfirmed && w.Person.Profile != null)//!w.LockoutEnabled &&
                    .Select(s => new UserProfileModel
                    {
                        UserId = s.Id,
                        UserName = s.UserName,
                        Family = s.Person.Profile.Family,
                        Name = s.Person.Profile.Name,
                        NationalCode = s.Person.Profile.NationalCode
                    }).ToList();


                //var query = _userRepository.Where()
                //        .Select(s => new 
                //        {
                //            s.Id,
                //            s.Person.Profile,
                //            s.UserName,
                //        }).AsEnumerable();
                //var userRoles = (from a in query
                //                 select new UserProfileModel()
                //                 {
                //                     Name = a.Profile.Name,
                //                     Family = a.Profile.Family,
                //                     UserId = a.Id,
                //                     UserName = a.
                //                     NationalCode = a.Profile.NationalCode
                //                 }).ToList();
                return new Tuple<bool, string, List<UserProfileModel>>(true, "خطا در انجام عملیات", userRoles);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<UserProfileModel>>(false, "خطا در انجام عملیات", null);
            }
        }
        public Tuple<bool, string, List<UserRoleModel>> GetUserRoles(long userId)
        {
            try
            {
                var query = _userRoleRepository.Where(w => w.UserId == userId && w.Role.RoleType != RoleType.AdminBranch &&
                        w.Role.RoleType != RoleType.AdminCentral && w.Role.RoleType != RoleType.AdminUniversity)
                        .Select(s => new
                        {
                            s.RoleId,
                            s.UserId,
                            s.Role.Name,
                            s.Role.NameFa
                        }).AsEnumerable();
                var userRoles = (from a in query
                                 select new UserRoleModel
                                 {
                                     RoleId = a.RoleId,
                                     UserId = a.UserId,
                                     RoleName = a.Name,
                                     RoleNameFa = a.NameFa
                                 }).ToList();
                return new Tuple<bool, string, List<UserRoleModel>>(true, "خطا در انجام عملیات", userRoles);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<UserRoleModel>>(false, "خطا در انجام عملیات", null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentNumber"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Tuple<bool, string, StudentProfileModel> GetStudentProfile(long studentNumber, long userId)
        {
            try
            {
                var b = _personRepository.Where(x => x.Student.StudentNumber == studentNumber)
                    .Include(i => i.Profile).Include(i => i.Student).FirstOrDefault();
                var person = (from p in _personRepository.Where(x => x.Student.StudentNumber == studentNumber)
                              .Include(i => i.Profile).Include(i => i.Student)
                              select new StudentProfileModel
                              {
                                  //PersonId = p.Id,
                                  Family = p.Profile.Family,
                                  Gender = p.Profile.Gender,
                                  Name = p.Profile.Name,
                                  NationalCode = p.Profile.NationalCode,
                                  FieldofStudyId = p.Student.FieldofStudyId,
                                  Grade = p.Student.Grade,
                                  MilitaryServiceStatus = p.Student.MilitaryServiceStatus,
                                  StudentNumber = p.Student.StudentNumber

                                  //StudentNumber = p.Student.StudentNumber,
                                  //                        Name = p.Profile.Name,
                                  //                        Family = p.Profile.Family,
                                  //                        FieldofStudyId = p.Student.FieldofStudyId,
                                  //                        Gender = p.Profile.Gender,
                                  //                        Grade = p.Student.Grade,
                                  //                        MilitaryServiceStatus = p.Student.MilitaryServiceStatus,
                                  //                        NationalCode = p.Profile.NationalCode,
                                  //                        NumberofRemainingUnits = 1,
                                  //                        NumberofSpentUnits = 1,
                                  //                        NumberofVotesCouncil = p.Requests.Count(r => r.Council != null)
                              }).FirstOrDefault();

                if (person == null)
                    return new Tuple<bool, string, StudentProfileModel>(false, "رکورد دانشجو یا پروفایل مورد نظر یافت نشد!", new StudentProfileModel());

                var fieldofStudies = _postPersonRepository.Where(p => p.PersonId == userId && p.Post.Signers.Any(s => s.RowNumber == 1))
                           .Select(f => f.FieldofStudyId).ToList();
                fieldofStudies.AddRange(_userPostRepository.Where(p => p.UserId == userId && p.Post.Signers.Any(s => s.RowNumber == 1))
                    .Select(x => x.FieldofStudyId).ToList());
                return !fieldofStudies.Contains(person.FieldofStudyId) ?
                    new Tuple<bool, string, StudentProfileModel>(false, "شما مجاز به ثبت درخواست برای دانشجوی مورد نظر نمی باشید!", new StudentProfileModel()) :
                    new Tuple<bool, string, StudentProfileModel>(true, "", person);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, StudentProfileModel>(false, "خطا در انجام عملیات!", new StudentProfileModel());
            }
        }
        /// <summary>
        /// مشخص می کند که دانشجو در لیست سمت ها و اختیارات پرسنل وجود دارد یا نه
        /// </summary>
        /// <param name="studentNumber"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Tuple<bool, string, RequestModel> IsStudentExist(long studentNumber, long userId)
        {
            try
            {
                var person = (from p in _personRepository.Where(x => x.Student.StudentNumber == studentNumber)
                               .Include(i => i.Profile).Include(i => i.Student.FieldofStudy.OrganizationStructureName)
                              select new RequestModel
                              {
                                  PersonId = p.Id,
                                  StudentNumber = p.Student.StudentNumber,
                                  Name = p.Profile.Name + " " + p.Profile.Family,
                                  Family = p.Profile.Family,
                                  FieldofStudyId = p.Student.FieldofStudyId,
                                  FieldofStudyString = p.Student.FieldofStudy.OrganizationStructureName.Name,
                                  Gender = p.Profile.Gender,
                                  Grade = p.Student.Grade,
                                  MilitaryServiceStatus = p.Student.MilitaryServiceStatus,
                                  NationalCode = p.Profile.NationalCode,
                                  NumberofRemainingUnits = 1,
                                  NumberofSpentUnits = 1,
                                  NumberofVotesCouncil = p.Requests.Count(r => r.Council != null)
                              }).FirstOrDefault();
                if (person == null)
                    return new Tuple<bool, string, RequestModel>(false, "رکورد دانشجو یا پروفایل مورد نظر یافت نشد!", new RequestModel());
                person.GenderString = person.Gender.GetDescription();
                person.GradeString = person.Grade.GetDescription();
                person.MilitaryServiceStatusString = person.MilitaryServiceStatus.GetDescription();

                var fieldofStudies = _postPersonRepository.Where(p => p.PersonId == userId && p.Post.Signers.Any(s => s.RowNumber == 1))
                           .Select(f => f.FieldofStudyId).ToList();
                fieldofStudies.AddRange(_userPostRepository.Where(p => p.UserId == userId && p.Post.Signers.Any(s => s.RowNumber == 1))
                    .Select(x => x.FieldofStudyId).ToList());
                return !fieldofStudies.Contains(person.FieldofStudyId) ?
                    new Tuple<bool, string, RequestModel>(false, "شما مجاز به ثبت درخواست برای دانشجوی مورد نظر نمی باشید!", new RequestModel()) :
                    new Tuple<bool, string, RequestModel>(true, "", person);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, RequestModel>(false, "خطا در انجام عملیات!", new RequestModel());
            }
        }

        public long GetPersonIdbyStudentNumber(long studentNumber)
        {
            var personId =
                _studentRepository.Where(s => s.StudentNumber == studentNumber).Select(s => s.PersonId).FirstOrDefault();
            return personId;
        }
    }
}
