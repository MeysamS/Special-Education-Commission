using System;
using System.Data.Entity;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.Enum;
using Comision.Service.IService;

namespace Comision.Service.ImplementationService
{
    public class MemberManagementService : IMemberManagementService
    {
        private readonly IMemberMasterRepository _memberMasterRepository;
        private readonly IMemberDetailsRepository _memberDetailsRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MemberManagementService(IMemberMasterRepository masterRepository,
            IMemberDetailsRepository detailsRepository, IRequestRepository requestRepository,
            IUnitOfWork unitOfWork)
        {
            _memberMasterRepository = masterRepository;
            _memberDetailsRepository = detailsRepository;
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public Tuple<bool, string> AddOrUpdateMaster(MemberMaster member, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                    _memberMasterRepository.Add(member);
                else
                {
                    // بررسی می کند که اگر برای این مستر قبلا کمیسیونی رای صادر شده است قابل حذف نباشد
                    if (_requestRepository.Contains(x => x.MemberMasterId == member.Id && x.Vote != null))
                        return new Tuple<bool, string>(false, "عملیات حذف امکانپذیر نمی باشد.اعضای مورد نظر در لیست درخواست ها وجود دارد ");

                    var memberMaster = _memberMasterRepository.Find(m => m.Id == member.Id);
                    if (memberMaster == null)
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");
                    memberMaster.Date = member.Date;
                    memberMaster.Name = member.Name;
                    _memberMasterRepository.Update(memberMaster);
                }

                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت");
            }
        }

        public Tuple<bool, string> AddOrUpdateDetail(MemberDetails details, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                    _memberDetailsRepository.Add(details);
                else
                {
                    var master =
                      _memberDetailsRepository.Where(x => x.Id == details.Id).Select(s => s.MemberMaster).FirstOrDefault();
                    if (master != null && _requestRepository.Contains(x => x.MemberMasterId == master.Id && x.Vote != null))
                        return new Tuple<bool, string>(false, "عملیات حذف امکانپذیر نمی باشد.اعضای مورد نظر در لیست درخواست ها وجود دارد ");

                    var memberDetail = _memberDetailsRepository.Find(m => m.Id == details.Id);
                    if (memberDetail == null)
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");
                    memberDetail.PersonName = details.PersonName;
                    memberDetail.PostName = details.PostName;
                    memberDetail.RowNumber = details.RowNumber;
                    _memberDetailsRepository.Update(memberDetail);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "ثبت شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت");
            }
        }

        public IQueryable<MemberMaster> GetMasters(long univercityId, RequestType requestType)
        {
            return _memberMasterRepository.Where(x => x.UniversityId == univercityId && x.RequestType == requestType);
        }

        public IQueryable<MemberDetails> GetDetail(long masterId)
        {
            return _memberDetailsRepository.Where(x => x.MemberMasterId == masterId);
        }

        public Tuple<bool, string> DeleteMaster(long id)
        {
            try
            {
                // بررسی می کند که اگر برای این مستر قبلا کمیسیونی رای صادر شده است قابل حذف نباشد
                if (_requestRepository.Contains(x => x.MemberMasterId == id && x.Vote!=null))
                    return new Tuple<bool, string>(false, "عملیات حذف امکانپذیر نمی باشد.اعضای مورد نظر در لیست درخواست ها وجود دارد ");

                if (_memberDetailsRepository.Contains(x => x.MemberMasterId == id))
                    return new Tuple<bool, string>(false, "عملیات حذف امکانپذیر نمی باشد. رکورد جاری دارای اعضاء تعریف شده می باشد.");

                _memberMasterRepository.Delete(x => x.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, " حذف با موفقیت انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "عملیات حذف با خطا مواجه شده است"); throw;
            }
        }

        public Tuple<bool, string> DeleteDetail(long id)
        {
            try
            {
                // بررسی می کند که اگر کمیسیونی بر اساس این اعضا برا ی انها رای صادر شده است قابل حذف نباشد
                var master =
                    _memberDetailsRepository.Where(x => x.Id == id).Select(s => s.MemberMaster).FirstOrDefault();
                if (master != null && _requestRepository.Contains(x => x.MemberMasterId == master.Id && x.Vote!=null))
                    return new Tuple<bool, string>(false, "عملیات حذف امکانپذیر نمی باشد.اعضای مورد نظر در لیست درخواست ها وجود دارد ");


                _memberDetailsRepository.Delete(x => x.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, " حذف با موفقیت انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "عملیات حذف با خطا مواجه شده است"); throw;
            }
        }
    }
}
