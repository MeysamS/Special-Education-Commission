using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Newtonsoft.Json.Linq;

namespace Comision.Service.ImplementationService
{
    public class StructureManageService : IStructureManageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrganizationStructureNameRepository _organizationStructureNameRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IEducationalGroupRepository _educationalGroupRepository;
        private readonly IFieldofStudyRepository _fieldofStudyRepository;
        private readonly IPostPersonRepository _postPersonRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserPostRepository _userPostRepository;
        
        public StructureManageService(IUnitOfWork unitOfWork,IOrganizationStructureNameRepository organizationStructure,
            ICollegeRepository collegeRepository,IUniversityRepository universityRepository,
            IEducationalGroupRepository educationalGroupRepository,IFieldofStudyRepository fieldofStudyRepository,
            IPostPersonRepository postPersonRepository,IStudentRepository studentRepository,
            IUserPostRepository userPostRepository)
        {
            _unitOfWork = unitOfWork;
            _organizationStructureNameRepository = organizationStructure;
            _collegeRepository = collegeRepository;
            _educationalGroupRepository = educationalGroupRepository;
            _fieldofStudyRepository = fieldofStudyRepository;
            _universityRepository = universityRepository;
            _postPersonRepository = postPersonRepository;
            _studentRepository = studentRepository;
            _userPostRepository = userPostRepository;
        }

        public Tuple<bool, string> AddorUpdateStructureComplete(StructureTreeModel model)
        {
            var exist = _organizationStructureNameRepository.Find(x => x.Name == model.Text);
            if (exist != null)
                return new Tuple<bool, string>(false, "خطا در انجام عملیات : عنوان وارد شده تکراریست");

            if (model.StructureType == StructureType.FieldofStudy)
                return new Tuple<bool, string>(false, "امکان ثبت اطلاعات برای سطح رشته آموزشی وجود ندارد");
            try
            {
                _unitOfWork.BeginTransaction();
                // ذخیره در جدول ساختار
                var org = new OrganizationStructureName { Name = model.Text };
                switch (model.StructureType)
                {
                    case StructureType.University:
                        org.StructureType = StructureType.College;
                        break;
                    case StructureType.College:
                        org.StructureType = StructureType.EducationalGroup;
                        break;
                    case StructureType.EducationalGroup:
                        org.StructureType = StructureType.FieldofStudy;
                        break;
                }

                var result = AddOrUpdateStructure(org, StateOperation.درج);
                // پایان ذخیره در جدول ساختار

                switch (model.StructureType)
                {
                    case StructureType.University:
                        var college = new College { UniversityId = model.ParentId, OrganizationStructureNameId = org.Id, OrganizationStructureName = org };
                        result = AddOrUpdateCollege(college, StateOperation.درج);
                        break;
                    case StructureType.College:
                        var edu = new EducationalGroup { CollegeId = model.ParentId, OrganizationStructureNameId = org.Id, OrganizationStructureName = org };
                        result = AddOrUpdateEducationGroup(edu, StateOperation.درج);
                        break;
                    case StructureType.EducationalGroup:
                        var fs = new FieldofStudy { EducationalGroupId = model.ParentId, OrganizationStructureNameId = org.Id, OrganizationStructureName = org };
                        result = AddOrUpdatefieldofStudy(fs, StateOperation.درج);
                        break;
                }
                _unitOfWork.CommitTransaction();
                return new Tuple<bool, string>(result.Item1, result.Item2);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                return new Tuple<bool, string>(true, "خطای سیستمی، لطفا با پشتیبانی شرکت تماس حاصب فرمائید");
            }
        }
        public Tuple<bool, string> AddOrUpdateStructure(OrganizationStructureName structure, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                {
                    var authentication = _organizationStructureNameRepository.Find(x => x.Name == structure.Name);
                    if (authentication != null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام مورد نظر تکراری می باشد");
                    }
                    _organizationStructureNameRepository.Add(structure);
                }
                else
                {
                    if (_organizationStructureNameRepository.Contains(x => x.Id != structure.Id && x.Name == structure.Name))
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام مورد نظر تکراری می باشد");
                    }
                    _organizationStructureNameRepository.Update(structure);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> AddOrUpdateCollege(College college, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                    _collegeRepository.Add(college);
                else
                    _collegeRepository.Update(college);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> AddOrUpdateEducationGroup(EducationalGroup educationalGroup, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                    _educationalGroupRepository.Add(educationalGroup);
                else
                    _educationalGroupRepository.Update(educationalGroup);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> AddOrUpdatefieldofStudy(FieldofStudy fieldofStudy, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                    _fieldofStudyRepository.Add(fieldofStudy);
                else
                    _fieldofStudyRepository.Update(fieldofStudy);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public IQueryable<OrganizationStructureName> GetStructure()
        {
            return _organizationStructureNameRepository.All();
        }
        public Tuple<bool, string, List<SubStructureModel>> GetSubStructurebyPost(PostType postType, long levelId)
        {
            try
            {
                var aaa = _collegeRepository.Where(u => u.UniversityId == levelId).ToList();

                var sss = (from a in _collegeRepository.Where(u => u.UniversityId == levelId).Include(i => i.OrganizationStructureName).AsEnumerable()
                           select
                               new SubStructureModel
                               {
                                   Id = a.Id,
                                   Name = a.OrganizationStructureName.Name,
                                   StructureType = StructureType.College
                               }).ToList();

                var listSubStructure = (postType == PostType.University
                    ? (from a in _universityRepository.Where(u => u.Id == levelId).AsEnumerable()
                       select
                           new SubStructureModel { Id = a.Id, Name = a.Name, StructureType = StructureType.University })
                    : (postType == PostType.College
                        ? (from a in _collegeRepository.Where(u => u.UniversityId == levelId)
                           .Include(i => i.OrganizationStructureName).AsEnumerable()
                           select
                               new SubStructureModel
                               {
                                   Id = a.Id,
                                   Name = a.OrganizationStructureName.Name,
                                   StructureType = StructureType.College
                               })
                        :(postType==PostType.EducationalGroup? (from a in
                            _educationalGroupRepository.Where(u => u.College.UniversityId == levelId)
                                .Include(i => i.OrganizationStructureName).AsEnumerable()
                           select
                               new SubStructureModel
                               {
                                   Id = a.Id,
                                   Name = a.OrganizationStructureName.Name,
                                   StructureType = StructureType.EducationalGroup
                               })
                          : (from a in
                             _fieldofStudyRepository.Where(u => u.EducationalGroup.College.UniversityId == levelId)
                                 .Include(i => i.OrganizationStructureName).AsEnumerable()
                             select
                                 new SubStructureModel
                                 {
                                     Id = a.Id,
                                     Name = a.OrganizationStructureName.Name,
                                     StructureType = StructureType.FieldofStudy
                                 })
                          ))).ToList();
                return new Tuple<bool, string, List<SubStructureModel>>(true, "", listSubStructure);
            }
            catch (Exception exception)
            {
                return new Tuple<bool, string, List<SubStructureModel>>(false, "خطا در لود اطلاعات", null);
            }
        }

        public Tuple<bool, string> Delete(StructureTreeModel model)
        {
            try
            {
                var exist = _organizationStructureNameRepository.Where(x => x.Id == model.OrgId).Any();
                if (!exist)
                    return new Tuple<bool, string>(false, "خطا در انجام عملیات، درخواست نامعتبر");
                _unitOfWork.BeginTransaction();
                switch (model.StructureType)
                {
                    case StructureType.College:
                        if (_postPersonRepository.Contains(x => x.CollegeId == model.Id))
                            return new Tuple<bool, string>(false, "خطا در انجام عملیات، انتساب سمت در این سطح انجام شده است!");
                        if (_educationalGroupRepository.Contains(x => x.CollegeId == model.Id))
                            return new Tuple<bool, string>(false, "خطا در انجام عملیات، این آیتم زیرمجموعه دارد!");
                        _collegeRepository.Delete(x => x.Id == model.Id);
                        _organizationStructureNameRepository.Delete(x => x.Id == model.OrgId);
                        break;
                    case StructureType.EducationalGroup:
                        if (_postPersonRepository.Contains(x => x.EducationalGroupId == model.Id))
                            return new Tuple<bool, string>(false, "خطا در انجام عملیات، انتساب سمت در این سطح انجام شده است!");
                        if (_fieldofStudyRepository.Contains(x => x.EducationalGroupId == model.Id))
                            return new Tuple<bool, string>(false, "خطا در انجام عملیات، این آیتم زیرمجموعه دارد!");
                        _educationalGroupRepository.Delete(x => x.Id == model.Id);
                        _organizationStructureNameRepository.Delete(x => x.Id == model.OrgId);
                        break;
                    case StructureType.FieldofStudy:
                        if (_studentRepository.Contains(x => x.FieldofStudyId == model.Id))
                            return new Tuple<bool, string>(false, "خطا در انجام عملیات، دانشجو در این رشته تعریف شده است!");
                        _fieldofStudyRepository.Delete(x => x.Id == model.Id);
                        _organizationStructureNameRepository.Delete(x => x.Id == model.OrgId);
                        break;
                }
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
                return new Tuple<bool, string>(true, "عملیات حذف انجام شد");
            }
            catch (Exception exception)
            {
                _unitOfWork.Rollback();
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public List<JObject> GetStructureByTreeView(long univercityId)
        {
            var tempData = (from uni in _universityRepository.Where(x => x.Id == univercityId)
                            select new
                            {
                                Univercity = uni,
                                Univercity_Colleges = uni.Colleges,
                                Univercity_Colleges_OrganizationStructureName = uni.Colleges.Select(x => x.OrganizationStructureName),
                                Colleges_EducationalGroups = uni.Colleges.Select(x => new { x.EducationalGroups }).DefaultIfEmpty(),
                                Colleges_EducationalGroups_OrganizationStructureName = uni.Colleges.Select(x => x.EducationalGroups.Select(c => c.OrganizationStructureName)),
                                Colleges_EducationalGroups_FieldofStudies = uni.Colleges.Select(x => x.EducationalGroups.Select(m => m.FieldofStudies)),
                                Colleges_EducationalGroups_FieldofStudies_OrganizationStructureName = uni.Colleges.Select(x => x.EducationalGroups.Select(m => m.FieldofStudies.Select(b => b.OrganizationStructureName)))
                            });
            var university = tempData.AsEnumerable().Select(m => m.Univercity).SingleOrDefault();

            var jobjects = new List<JObject>();
            var root = new JObject { { "id", university?.Id ?? 0 }, { "text", university?.Name }, { "hasChild", university?.Colleges != null }, { "structureType", (int)StructureType.University }, { "parentId", university?.Id } };
            var jarray = new JArray();
            if (university?.Colleges != null)
                foreach (var college in university.Colleges)
                {
                    var jarray2 = new JArray();
                    var jCollage = new JObject { { "id", college.Id }, { "text", college.OrganizationStructureName.Name }, { "hasChild", college.EducationalGroups != null }, { "structureType", (int)StructureType.College }, { "parentId", university.Id }, { "orgId", college.OrganizationStructureNameId } };
                    if (college.EducationalGroups != null)
                        foreach (var educationalGroup in college.EducationalGroups)
                        {
                            var jarray3 = new JArray();
                            var jeducationGroup = new JObject
                            {{ "id", educationalGroup.Id},{"text", educationalGroup.OrganizationStructureName.Name},{ "hasChild", educationalGroup.FieldofStudies != null},{ "structureType", (int)StructureType.EducationalGroup},{ "parentId", college.Id},{"orgId",educationalGroup.OrganizationStructureNameId}};
                            if (educationalGroup.FieldofStudies != null)
                                foreach (var jfieldOfstudy in educationalGroup.FieldofStudies.Select(fieldOfstudy => new JObject { { "id", fieldOfstudy.Id }, { "text", fieldOfstudy.OrganizationStructureName.Name }, { "hasChild", fieldOfstudy.EducationalGroup.FieldofStudies != null }, { "structureType", (int)StructureType.FieldofStudy }, { "parentId", college.Id }, { "orgId", fieldOfstudy.OrganizationStructureNameId } }))
                                {
                                    jarray3.Add(jfieldOfstudy);
                                }
                            jeducationGroup.Add("children", jarray3);
                            jarray2.Add(jeducationGroup);
                        }
                    jCollage.Add("children", jarray2);
                    jarray.Add(jCollage);
                }
            root.Add("children", jarray);
            jobjects.Add(root);
            return jobjects;
        }
        public College FindCollegeInEducationalGroup(long id)
        {

            return _collegeRepository.Where(x => x.Id == id).Include(x => x.EducationalGroups).SingleOrDefault();
        }
        public EducationalGroup FindEducationalGroup(long id)
        {
            return _educationalGroupRepository.Where(x => x.Id == id).Include(x => x.FieldofStudies).SingleOrDefault();
        }
        public FieldofStudy FindFieldofStudy(long id)
        {
            return _fieldofStudyRepository.Find(x => x.Id == id);
        }
        public Tuple<bool, string, University> FindUniversity(long universityId)
        {
            try
            {
                var query = _universityRepository.Find(s => s.Id == universityId);
                return new Tuple<bool, string, University>(true, "عملیات با موفیت انجام شد", query);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, University>(true, "عملیات با مشکل مواجه شده است", null);
            }
        }

        /// <summary>
        /// گرفتن لیست تمام گروه های آموزشی مربوط به انشگاه خاص
        /// </summary>
        /// <param name="collegeId"></param>
        /// <returns></returns>
        public Tuple<bool, string, List<EducationalGroup>> GetAllEducationalGroup(long collegeId)
        {
            try
            {
                var query = _educationalGroupRepository.Where(s => s.CollegeId == collegeId).
                    Include(i => i.OrganizationStructureName).ToList();
                return new Tuple<bool, string, List<EducationalGroup>>(true, "عملیات با موفیت انجام شد", query);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<EducationalGroup>>(true, "عملیات با مشکل مواجه شده است", null);
            }
        }

        public Tuple<bool, string, IQueryable<EducationalGroup>> WhereEducationalGroup(Expression<Func<EducationalGroup, bool>> predicate)
        {
            try
            {
                var query = _educationalGroupRepository.Where(predicate);
                return new Tuple<bool, string, IQueryable<EducationalGroup>>(true, "عملیات با موفیت انجام شد", query);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, IQueryable<EducationalGroup>>(true, "عملیات با مشکل مواجه شده است", null);
            }
        }
        public Tuple<bool, string, List<FieldofStudy>> GetAllFieldofStudy()
        {
            try
            {
                var query = _fieldofStudyRepository.All().Include(i => i.OrganizationStructureName).ToList();
                return new Tuple<bool, string, List<FieldofStudy>>(true, "عملیات با موفیت انجام شد", query);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, List<FieldofStudy>>(true, "عملیات با مشکل مواجه شده است", null);
            }
        }

        /// <summary>
        /// لیست رشته های تحصیلی کاربر که در جداول سمت یا اختیارات تعریف شده است
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        public Tuple<bool, string, List<FieldofStudy>> GetAllFieldofStudy(long userId, int rowNumber)
        {
            try
            {
                 var fieldofStudies = _postPersonRepository.Where(p => p.PersonId == userId && p.Post.Signers.Any(s => s.RowNumber == rowNumber))
                         .Select(d=>d.FieldofStudy).Include(i => i.OrganizationStructureName).ToList();
                
                fieldofStudies.AddRange(_userPostRepository.Where(p => p.UserId == userId && p.Post.Signers.Any(s => s.RowNumber == rowNumber))
                    .Select(f => f.FieldofStudy).Include(i => i.OrganizationStructureName).ToList());
            
                return new Tuple<bool, string, List<FieldofStudy>>(true, "عملیات با موفیت انجام شد", fieldofStudies.DistinctBy(d => d.Id).ToList());
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, List<FieldofStudy>>(false, "عملیات با مشکل مواجه شده است", null);
            }
        }
    }
}
