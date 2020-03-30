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

namespace Comision.Service.ImplementationService
{
    public class BaseInfoComissionCouncilService : IBaseInfoComissionCouncilService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpecialEducationRepository _specialEducationRepository;
        private readonly ISignerRepository _signerRepository;
        private readonly ICommissionSpecialEducationRepository _commissionSpecialEducationRepository;
        public BaseInfoComissionCouncilService(IUnitOfWork unitOfWork, ISignerRepository signerRepository,
            ISpecialEducationRepository specialEducationRepository,
            ICommissionSpecialEducationRepository commissionSpecialEducationRepository)
        {
            _unitOfWork = unitOfWork;
            _specialEducationRepository = specialEducationRepository;
            _signerRepository = signerRepository;
            _commissionSpecialEducationRepository = commissionSpecialEducationRepository;
        }
        public Tuple<bool, string> AddOrUpdateSpecialEducation(SpecialEducationModel specialEducationModel, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                {
                    var specialEducation = _specialEducationRepository.Find(x => x.Name == specialEducationModel.Name);
                    if (specialEducation != null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام مورد نظر تکراری می باشد");
                    }
                    SpecialEducation newAuthentication = new SpecialEducation
                    {
                        Name = specialEducationModel.Name
                    };
                    _specialEducationRepository.Add(newAuthentication);
                }
                else
                {
                    if (_specialEducationRepository.Contains(x => x.Id != specialEducationModel.Id && x.Name == specialEducationModel.Name))
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : نام مورد نظر تکراری می باشد");
                    }
                    var authentication = _specialEducationRepository.Find(x => x.Id == specialEducationModel.Id);
                    if (authentication == null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");
                    }
                    authentication.Name = specialEducationModel.Name;
                    _specialEducationRepository.Update(authentication);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }
        public Tuple<bool, string> AddOrUpdateSigner(SignerModel signerModel, StateOperation stateOperation)
        {
            try
            {
                if (stateOperation == StateOperation.درج)
                {
                    var signer = _signerRepository.Find(x => x.RequestType == signerModel.RequestType && x.RowNumber == signerModel.RowNumber);
                    if (signer != null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر تکراری می باشد");
                    }
                    Signer newSigner = new Signer
                    {
                        PostId = signerModel.PostId,
                        RequestType = signerModel.RequestType,
                        RowNumber = signerModel.RowNumber
                    };
                    _signerRepository.Add(newSigner);
                }
                else
                {
                    if (_signerRepository.Contains(x => x.Id != signerModel.Id && x.RequestType == signerModel.RequestType && x.RowNumber == signerModel.RowNumber))
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر تکراری می باشد");
                    }
                    var signer = _signerRepository.Find(x => x.Id == signerModel.Id);
                    if (signer == null)
                    {
                        return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");
                    }
                    signer.PostId = signerModel.PostId;

                    signer.RowNumber = signerModel.RowNumber;

                    _signerRepository.Update(signer);
                }
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, "عملیات ثبت شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(false, "خطا در انجام عملیات");
            }
        }

        //public Tuple<bool, string> AddOrUpdateSigner2(SignerModel signerModel, StateOperation stateOperation)
        //{
        //    try
        //    {
        //        if (_signerRepository.Contains(x => x.Id != signerModel.Id && x.RequestType == signerModel.RequestType && x.RowNumber == signerModel.RowNumber))
        //            return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر تکراری می باشد");

        //        var newSigner = new Signer()
        //        {
        //            PostId = signerModel.PostId,
        //            RequestType = signerModel.RequestType,
        //            RowNumber = signerModel.RowNumber
        //        };

        //        if (stateOperation == StateOperation.درج)
        //            _signerRepository.Add(newSigner);
        //        else 
        //        {
        //            if (_signerRepository.Contains(x => x.Id == signerModel.Id)==false)
        //                return new Tuple<bool, string>(false, "خطا در انجام عملیات : رکورد مورد نظر یافت نشد");

        //            if (signerModel.Id != null) newSigner.Id = (long)signerModel.Id;
        //            _signerRepository.Update(newSigner);
        //        }
        //        _unitOfWork.SaveChanges();
        //        return new Tuple<bool, string>(true, "عملیات ثبت شد");
        //    }
        //    catch (Exception)
        //    {
        //        return new Tuple<bool, string>(false, "خطا در انجام عملیات");
        //    }
        //}

        public List<SpecialEducation> GetAllSpecialEducation()
        {
            return _specialEducationRepository.All().ToList();
        }
        public List<SpecialEducation> GetAllSpecialEducationforRequest(long requestId)
        {
            return _commissionSpecialEducationRepository.Where(s => s.CommissionId == requestId)
                .Select(s => s.SpecialEducation).ToList();
        }
        public List<Signer> GetAllSigner(RequestType requestType)
        {
            return _signerRepository.Where(s => s.RequestType == requestType).ToList();
        }
        public List<Signer> WhereSigner(Expression<Func<Signer, bool>> predicate)
        {
            return _signerRepository.Where(predicate).Include(i => i.Post).ToList();
        }
        public Tuple<bool, string> DeleteSpecialEducation(long id)
        {
            try
            {
                if (_commissionSpecialEducationRepository.Contains(d => d.SpecialEducationId == id))
                    return new Tuple<bool, string>(true, "رکورد مورد نظر قبلا استفاده شده است ، لذا قابل حذف نیست");
                _specialEducationRepository.Delete(p => p.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, " حذف با موفقیت انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(true, "عملیات حذف با خطا مواجه شده است");
            }
        }
        public Tuple<bool, string> DeleteSigner(long id)
        {
            try
            {
                _signerRepository.Delete(p => p.Id == id);
                _unitOfWork.SaveChanges();
                return new Tuple<bool, string>(true, " حذف با موفقیت انجام شد");
            }
            catch (Exception)
            {
                return new Tuple<bool, string>(true, "عملیات حذف با خطا مواجه شده است");
            }
        }
    }
}
