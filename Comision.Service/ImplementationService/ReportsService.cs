using System;
using System.Data.Entity;
using System.Linq;
using Comision.Data.Context;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Service.Model.PrintModel;
using Comision.Utility;
using Comision.Service.Enum;

namespace Comision.Service.ImplementationService
{
    /// <summary>
    /// گزارشات
    /// </summary>
    public class ReportsService : IReportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestRepository _requestRepository;
        private readonly IMemberMasterRepository _memberMasterRepository;
        private readonly ICouncilService _councilService;
        private readonly ISignerRepository _signerRepository;
        private readonly ICartableRepository _cartableRepository;
        private readonly IUniversityRepository _universityRepository;

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public ReportsService(IUnitOfWork unitOfWork, IRequestRepository requestRepository, IMemberMasterRepository memberMasterRepository,
            IUniversityRepository universityRepository, ICouncilService councilService, ISignerRepository signerRepository, ICartableRepository cartableRepository)

        {
            _requestRepository = requestRepository;
            _memberMasterRepository = memberMasterRepository;
            _unitOfWork = unitOfWork;
            _councilService = councilService;
            _signerRepository = signerRepository;
            _cartableRepository = cartableRepository;
            _universityRepository = universityRepository;
        }

        /// <summary>
        /// چاپ کمیسیون
        /// </summary>
        /// <param name="commissionId" />
        /// <param name="addressUrlFile">ادرس  عکس های امضا الکترونیکی</param>
        public PCommissionModel PrintCommission(long commissionId, AddressUrlFile addressUrlFile)
        {
            try
            {
                // اگر دانشجو مرد باشد و وضعیت سربازی آن مشمول باشد این متغیر (درست= ترو) خواهد شد
                // وقتی این متغیر (درست = ترو) باشد امضا کنند ردیف اخر نشان داده می شود.
                var query = (_requestRepository.Where(
                    c => c.RequestType == RequestType.Comision && c.Id == commissionId)
                    .Select(item => new PCommissionModel
                    {
                        PRequestDataModel =
                            new PRequestDataModel
                            {
                                RequestId = item.Id,
                                CommissionCouncilNumber = item.Commission.CommissionNumber,
                                Date = item.Vote != null ? item.Vote.DateVote : (DateTime?)null,
                                Description = item.Commission.Description,
                                DescriptionVerdict = item.Cartables.FirstOrDefault(d => d.CartableValidation == CartableValidation.Valid && d.CartableStatus == CartableStatus.Verdict).Description,
                                VoteText = item.Vote != null ? item.Vote.VoteText : "",
                                RowNumber = item.Commission.RowNumber,
                                ////RefertoText = item.Vote.ReferText, قبلا در صدوری رای بوده است
                                /// این متن در واقعی متنی است که در مرحله اخر برای سنوات بررسی شده است
                                RefertoText=item.Cartables.Where(sel=>sel.CartableValidation==CartableValidation.Valid && sel.RowNumber==6).Select(s => s.Description).FirstOrDefault() ?? " "
                            },

                        PMemberDetailsModel =
                            item.MemberMaster.MemberDetails.Select(
                                i =>
                                    new PMemberDetailsModel
                                    {
                                        PostName = i.PostName,
                                        FullName = i.PersonName,
                                        RowNumber = i.RowNumber
                                    }).ToList(),
                        PSpecialEducationModel =
                            item.Commission.CommissionSpecialEducations.Select(
                                i =>
                                    new PSpecialEducationModel
                                    {
                                        Name = i.SpecialEducation.Name,
                                        State = true,
                                        Id = i.SpecialEducationId
                                    }).ToList(),
                        PStudentInformationModel = new PStudentInformationModel
                        {
                            NameFamili = item.Person.Profile.Name + " " + item.Person.Profile.Family,
                            StudentNumber = item.Person.Student.StudentNumber,
                            FieldofStudyName = item.Person.Student.FieldofStudy.OrganizationStructureName.Name,
                            Grade = item.Person.Student.Grade,
                            // GradeName = item.Person.Student.Grade.GetDescription(),
                            NumberofRemainingUnits = item.NumberofRemainingUnits,
                            NumberofSpentUnits = item.NumberofSpentUnits,
                            MilitaryServiceStatus = item.Person.Student.MilitaryServiceStatus,
                            Gender = item.Person.Profile.Gender
                            // MilitaryServiceStatusName = item.Person.Student.MilitaryServiceStatus.GetDescription()
                        }

                    })).FirstOrDefault();
                if (query != null)
                {
                    query.PStudentInformationModel.GradeName = query.PStudentInformationModel.Grade.GetDescription();
                    query.PStudentInformationModel.MilitaryServiceStatusName =
                    query.PStudentInformationModel.MilitaryServiceStatus.GetDescription();
                    // قبلا برای مشمول و مرد بدن استفاده می شد.
                    //////bool showMilitaryServiceStatus = query.PStudentInformationModel.MilitaryServiceStatus == MilitaryServiceStatus.Included
                    //////    && query.PStudentInformationModel.Gender == Gender.Male;
                    // ایا امضا معاون دانشجویی نشان داده شود
                    bool showRefrenceTo =
                        _cartableRepository.Contains(
                            i =>
                                i.RequestId == commissionId &&
                                i.CartableValidation == CartableValidation.Valid && i.RowNumber == 6);



                    var querySigners = (from singer in _signerRepository.Where(i => i.RequestType == RequestType.Comision && (showRefrenceTo || i.RowNumber != 6))
                                        join itemsinger in _cartableRepository.Where(i => i.RequestId == commissionId && i.CartableValidation == CartableValidation.Valid)
                                       on new { singer.PostId, singer.RowNumber } equals new { itemsinger.PostId, itemsinger.RowNumber } into signerCartablsSiners
                                        from data in signerCartablsSiners.DefaultIfEmpty()
                                        select new PSignersListModel
                                        {
                                            PostId = singer.PostId,
                                            RowNumber = singer.RowNumber,
                                            SignatureUrl = data.Person.Personel.Signature != null ? addressUrlFile.Signature + data.Person.Personel.Signature : "",
                                            PostName = singer.Post.Name
                                        }).ToList();
                    query.PSignersModel = new PSignersModel(querySigners);
                    ////query.PSignersModel = new PSignersModel(query.PSignersListModel);
                    return query;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public PCouncilModel PrintCouncil(long councilId, AddressUrlFile addressUrlFile)
        {
            try
            {
                var query = (_requestRepository.Where(
              c => c.RequestType == RequestType.Council && c.Id == councilId)
              .Select(item => new PCouncilModel
              {
                  PRequestDataModel =
                      new PRequestDataModel
                      {
                          RequestId = item.Id,
                          CommissionCouncilNumber = item.Council.CouncilNumber,
                          Date = item.Council.Date,
                          Description = item.Council.Description,
                          ProblemText = item.Council.ProblemText,
                          VoteText = item.Vote.VoteText,
                          RowNumber = item.Council.RowNumber,
                          //  RefertoText = item.Vote.ReferText,
                          RefertoText=item.Cartables.Where(sel=>sel.CartableValidation==CartableValidation.Valid && sel.RowNumber==7).Select(s=>s.Description).FirstOrDefault()??" "
                      },


                  PMemberDetailsModel =
                      item.MemberMaster.MemberDetails.Select(
                          i =>
                              new PMemberDetailsModel
                              {
                                  PostName = i.PostName,
                                  FullName = i.PersonName,
                                  RowNumber = i.RowNumber
                              }).ToList(),


                  PStudentInformationModel = new PStudentInformationModel
                  {
                      NameFamili = item.Person.Profile.Name + " " + item.Person.Profile.Family,
                      StudentNumber = item.Person.Student.StudentNumber,
                      FieldofStudyName = item.Person.Student.FieldofStudy.OrganizationStructureName.Name,
                      Grade = item.Person.Student.Grade,
                      //GradeName = item.Person.Student.Grade.GetDescription(),
                      NumberofRemainingUnits = item.NumberofRemainingUnits,
                      NumberofSpentUnits = item.NumberofSpentUnits,
                      MilitaryServiceStatus = item.Person.Student.MilitaryServiceStatus,
                      StudentId = item.Person.Id
                      // MilitaryServiceStatusName = item.Person.Student.MilitaryServiceStatus.GetDescription()
                  }

              })).FirstOrDefault();
                if (query != null)
                {
                    query.PStudentInformationModel.GradeName = query.PStudentInformationModel.Grade.GetDescription();
                    query.PStudentInformationModel.MilitaryServiceStatusName =
                        query.PStudentInformationModel.MilitaryServiceStatus.GetDescription();

                    bool showRefrenceTo =
                  _cartableRepository.Contains(
                      i =>
                          i.RequestId == councilId &&
                          i.CartableValidation == CartableValidation.Valid && i.RowNumber == 6);


                    var querySigners = (from singer in _signerRepository.Where(i => i.RequestType == RequestType.Council && (showRefrenceTo || i.RowNumber != 6))
                                        join itemsinger in _cartableRepository.Where(i => i.RequestId == councilId && i.CartableValidation == CartableValidation.Valid)
                                       on new { singer.PostId, singer.RowNumber } equals new { itemsinger.PostId, itemsinger.RowNumber } into signerCartablsSiners
                                        from data in signerCartablsSiners.DefaultIfEmpty()
                                        select new PSignersListModel
                                        {
                                            PostId = singer.PostId,
                                            RowNumber = singer.RowNumber,
                                            SignatureUrl = data.Person.Personel.Signature != null ? addressUrlFile.Signature + data.Person.Personel.Signature : "",
                                            PostName = singer.Post.Name
                                        }).ToList();
                    query.PSignersModel = new PSignersModel(querySigners);

                    ////query.PSignersModel = new PSignersModel(query.PSignersListModel);

                    query.PStudentInformationModel.CountuseCouncil =
                        _councilService.GetNumberofVotesCouncil(query.PStudentInformationModel.StudentId);
                    return query;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public PCommissionCouncilAllModel PrintCommissionAll(string searcvalue, SearchType searchtype)
        {
            try
            {
                Int64 number = (searchtype == SearchType.FileNumber || searchtype == SearchType.StudentNumber)
                    ? Convert.ToInt64(searcvalue) : 0;

                var queryrequest =
                    _requestRepository.Where(
                        s => (s.RequestType == RequestType.Comision) &&
                            (searchtype == SearchType.StudentNumber)
                                ? s.Person.Student.StudentNumber == number
                                : (searchtype == SearchType.StudentNameFamili)
                                    ? (s.Person.Profile.Name + " " + s.Person.Profile.Family).Contains(searcvalue)

                                    : (searchtype == SearchType.FileNumber) && s.Commission.CommissionNumber == number);




                var firstOrDefault = queryrequest.Include(d => d.MemberMaster.MemberDetails).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    PCommissionCouncilAllModel query = new PCommissionCouncilAllModel
                    {
                        PRequestAllDataModels = _requestRepository.Where(
                        s => (s.RequestType == RequestType.Comision) &&
                            (searchtype == SearchType.StudentNumber)
                                ? s.Person.Student.StudentNumber == number
                                : (searchtype == SearchType.StudentNameFamili)
                                    ? (s.Person.Profile.Name + " " + s.Person.Profile.Family).Contains(searcvalue)

                                    : (searchtype == SearchType.FileNumber) && s.Commission.CommissionNumber == number)

                            .Select(item => new PRequestAllDataModel
                            {
                                Date = item.Commission.Date,
                                StudentNumber = item.Person.Student.StudentNumber,
                                VoteText = item.Vote == null ? " " : item.Vote.VoteText,
                                CommissionCouncilNumber = item.Commission.CommissionNumber,
                                NameFamili = item.Person.Profile.Name + " " + item.Person.Profile.Family,
                                RequestId = item.Id,
                                PSpecialEducationModel = item.Commission.CommissionSpecialEducations.Select(i =>
                                    new PSpecialEducationModel
                                    {
                                        Name = i.SpecialEducation.Name,
                                        State = true,
                                        Id = i.SpecialEducationId
                                    }).ToList()

                            }).ToList(),
                        PMemberDetailsModel = firstOrDefault
                            .MemberMaster.MemberDetails.Select(i => new PMemberDetailsModel
                            {
                                PostName = i.PostName,
                                FullName = i.PersonName,
                                RowNumber = i.RowNumber
                            }
                            ).ToList()
                    };
                    if (query.PRequestAllDataModels != null)
                    {
                        foreach (PRequestAllDataModel item in query.PRequestAllDataModels)
                        {
                            item.Description =
                                string.Join("\r\n",
                                    item.PSpecialEducationModel.Select(d => d.Name).ToArray());
                        }
                    }

                    return query;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public PCommissionCouncilAllModel PrintCouncilAll(string searcvalue, SearchType searchtype)
        {
            try
            {
                Int64 number = (searchtype == SearchType.FileNumber || searchtype == SearchType.StudentNumber)
                    ? Convert.ToInt64(searcvalue) : 0;

                var queryrequest =
                    _requestRepository.Where(
                        s => (s.RequestType==RequestType.Council) &&
                            (searchtype == SearchType.StudentNumber)
                                ? s.Person.Student.StudentNumber == number
                                : (searchtype == SearchType.StudentNameFamili)
                                    ? (s.Person.Profile.Name + " " + s.Person.Profile.Family).Contains(searcvalue)

                                    : (searchtype == SearchType.FileNumber) && s.Council.CouncilNumber == number);


                var firstOrDefault = queryrequest.Include(d => d.MemberMaster.MemberDetails).FirstOrDefault();
                if (firstOrDefault != null)
                {
                  
                    PCommissionCouncilAllModel query = new PCommissionCouncilAllModel
                    {
                        PRequestAllDataModels = _requestRepository.Where(
                        s => (s.RequestType == RequestType.Council) && (
                                 (searchtype == SearchType.StudentNumber)
                                     ? s.Person.Student.StudentNumber == number
                                     : (searchtype == SearchType.StudentNameFamili)
                                         ? (s.Person.Profile.Name + " " + s.Person.Profile.Family).Contains(searcvalue)

                                         : (searchtype == SearchType.FileNumber) && s.Council.CouncilNumber == number))

                            .Select(item => new PRequestAllDataModel
                            {
                                Date = item.Council.Date,
                                StudentNumber = item.Person.Student.StudentNumber,
                                VoteText = item.Vote == null ? " " : item.Vote.VoteText,
                                CommissionCouncilNumber = item.Council.CouncilNumber,
                                NameFamili = item.Person.Profile.Name + " " + item.Person.Profile.Family,
                                RequestId = item.Id,
                                Description = item.Council.ProblemText

                            }).ToList(),
                        PMemberDetailsModel = firstOrDefault
                            .MemberMaster.MemberDetails.Select(i => new PMemberDetailsModel
                            {
                                PostName = i.PostName,
                                FullName = i.PersonName,
                                RowNumber = i.RowNumber
                            }
                            ).ToList()
                    };


                    return query;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public string GetLogoUrl(AddressUrlFile addressUrl)
        {
            try
            {
                var logoname = _universityRepository.All().Select(sel => sel.Logo).FirstOrDefault();
                return addressUrl.Logo + logoname;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
