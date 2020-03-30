using System;
using System.Linq;
using System.Web.Mvc;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Service.Model.PrintModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using Comision.Helper.Filters;
using System.ComponentModel;
using Comision.Service.ImplementationService;
using Comision.Utility;
using Comision.Service.Enum;

namespace Comision.Ui.Areas.Personel.Controllers
{
    [AuthorizeUser]
    [Description("گزارشات")]
    public partial class ReportController : Controller
    {
        // Stimulsoft.Report.StiConfig.LoadLocalization(Server.MapPath("~/Reports/fa.xml"));
        private readonly string _lanFile;
        private readonly IReportsService _reportsService;
        private readonly IRequestService _requestService;
        private readonly ICommissionService _commissionService;
        private readonly ICouncilService _councilService;
      
        public ReportController(IReportsService reportsService, IRequestService requestService,
            ICommissionService commissionService, ICouncilService councilService)
        {
            _reportsService = reportsService;
            _requestService = requestService;
            _commissionService = commissionService;
            _councilService = councilService;
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.ListRequestStudent, AllowInAccess = true)]
        public virtual ActionResult ListRequestStudent()
        {
            return View();
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.ListRequestStudent, AllowInAccess = false)]
        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListRequestStudent(long studentNumber)
        {
            var listRequestStudent = _requestService.ListRequestStudent(studentNumber).Item3;
            var ja = new JArray();
            foreach (var itemObject in listRequestStudent.Select(item => new JObject
            {
              {"Id",item.Id},
              {"DateVote",item.Vote!=null?item.Vote.DateVote.ToPeString():""},
              {"RequestType",item.RequestType.GetDescription()},
              {"TextVote",item.Vote!=null?item.Vote.VoteText:"رای صدار نشده است"},
              {"VoteType",item.Vote!=null?item.Vote.VoteType.GetDescription():""},
              {"VoteStatus",item.Vote!=null?item.Vote.VoteStatus.GetDescription():""}
            }))
            {
                ja.Add(itemObject);
            }
            var jo = new JObject { { "rows", ja } };
            //jo.Add("total", pagesCount);
            return Content(JsonConvert.SerializeObject(jo));
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.CommisionAll, AllowInAccess = true)]
        public virtual ActionResult CommisionAll()
        {
            var searchType = new SelectList(EnumerationService.GetEnumValues<SearchType>(), "Value", "Text");
            TempData["SearchType"] = searchType;
            return View();
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.CouncilAll, AllowInAccess = true)]
        public virtual ActionResult CouncilAll()
        {
            var searchType = new SelectList(EnumerationService.GetEnumValues<SearchType>(), "Value", "Text");
            TempData["SearchType"] = searchType;
            return View();
        }

        /// <summary>
        /// گزارش بر اساس شماره کمیسون و یا شماره شورا
        /// </summary>
        /// <param name="commisionOrCouncilNumber"></param>
        /// <returns></returns>

        //public virtual ActionResult PrintCommisionOrCouncilAll(long commisionOrCouncilNumber)
        //{
        //    try
        //    {
        //        var request = _requestService.Find(commisionOrCouncilNumber);
        //        if (request.RequestType == RequestType.Comision)
        //        {
        //            TempData["Number"] = (long)commisionOrCouncilNumber;
        //            ViewBag.NameAction = "GetReportAllCommission";
        //        }
        //        else
        //        {
        //            TempData["Number"] = (long)councilNumberId;
        //            ViewBag.NameAction = "GetReportAllCouncil";
        //        }
        //        ////TempData["Number"] = (long)212;
        //        return View("ReportShow");
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { isError = true, Message = "خطا در بارگزاری درخواست ها!" });
        //    }

        //}

        /// <summary>
        /// گزارش یک کمیسیون خاص
        /// </summary>
        /// <param name="commisionId"></param>
        /// <returns></returns>
        [CustomAuthorize(RoleNameDefault = AccessLevel.PrintCommision, AllowInAccess = true)]
        public virtual ActionResult PrintCommision(long commisionId)
        {
            TempData["RequestId"] = commisionId;
            ViewBag.NameAction = "GetReportCommission";
            ////TempData["Number"] = (long)212;
            return View("ReportShow");
        }

        /// <summary>
        /// گزارش یک شورای خاص
        /// </summary>
        /// <param name="councilId"></param>
        /// <returns></returns>
        [CustomAuthorize(RoleNameDefault = AccessLevel.PrintCouncil, AllowInAccess = true)]
        public virtual ActionResult PrintCouncil(long councilId)
        {
           
            TempData["RequestId"] = councilId;
            ViewBag.NameAction = "GetReportCouncil";
            ////TempData["Number"] = (long)212;
            return View("ReportShow");
        }

        /// <summary>
        /// گزارش بر اساس شماره کمیسیون ها
        /// </summary>
        /// <param name="commisionId"></param>
        /// <returns></returns>
        [CustomAuthorize(RoleNameDefault = AccessLevel.CommisionAll, AllowInAccess = false)]
        public virtual ActionResult PrintCommisionAll(string searcvalue, int searchtype)
        {
            TempData["searcvalue"] = searcvalue;
            TempData["searchtype"] = ((SearchType)searchtype);
            ViewBag.NameAction = "GetReportAllCommission";
            ////TempData["Number"] = (long)212;
            return View("ReportShow");
        }

        /// <summary>
        /// گزارش براساس شماره شورا
        /// </summary>
        /// <param name="councilId"></param>
        /// <returns></returns>
        [CustomAuthorize(RoleNameDefault = AccessLevel.CouncilAll, AllowInAccess = false)]
        public virtual ActionResult PrintCouncilAll(string searcvalue,int searchtype)
        {
            TempData["searcvalue"] = searcvalue;
            TempData["searchtype"] = ((SearchType)searchtype);

            ViewBag.NameAction = "GetReportAllCouncil";
            ////TempData["Number"] = (long)212;
            return View("ReportShow");
        }
        // GET: Personel/Report
        [CustomAuthorize(RoleNameDefault = AccessLevel.PrintCommision, AllowInAccess = false)]
        public virtual ActionResult GetReportCommission()
        {
          var lanFile= new AddressUrlFile(Server.MapPath("~/"));
            if (System.IO.File.Exists(lanFile.Localizationfa))
                StiConfig.LoadLocalization(lanFile.Localizationfa);
            var idrequest = (long)TempData["RequestId"];
            var query = _reportsService.PrintCommission(idrequest, new AddressUrlFile(Server.MapPath("~/")));

            if (query == null) return View(MVC.Errors.Views.Error);

            //StiReport report = new StiReportCompiledClass();
            var mainreport = new StiReport();
            mainreport.RegBusinessObject("RequestData", query.PRequestDataModel);
            mainreport.RegBusinessObject("Signers", query.PSignersModel);
            mainreport.RegBusinessObject("MemberDetails", query.PMemberDetailsModel.OrderBy(f => f.RowNumber).Take(6).ToList());
            //query.PSpecialEducationModel.Add(new PSpecialEducationModel { Id = 3, Name = "زایمان", State = true });
            mainreport.RegBusinessObject("SpecialEducation", query.PSpecialEducationModel.ToList());
            mainreport.RegBusinessObject("StudentInformation", query.PStudentInformationModel);

            mainreport.Load(Server.MapPath("~/Reports/ReportCommission.mrt"));

            mainreport.Render(true);
            mainreport.Compile();
            mainreport["NumberPrint"] = "1";
            mainreport["LogoPrint"] = _reportsService.GetLogoUrl(new AddressUrlFile(Server.MapPath("~/")));
            mainreport["DatePrint"] = DateTime.Now;
            return StiMvcViewerFx.GetReportSnapshotResult(mainreport);
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.CommisionAll, AllowInAccess = false)]
        public virtual ActionResult GetReportAllCommission()
        {
            string _lanFile = Server.MapPath("~/Reports/fa.xml");
            if (System.IO.File.Exists(_lanFile))
                StiConfig.LoadLocalization(_lanFile);

            string searcvalue = (string)TempData["searcvalue"];
            SearchType searchtype = (SearchType)TempData["searchtype"];
            var query = _reportsService.PrintCommissionAll(searcvalue, searchtype);

            var mainreport = new StiReport();
            if (query == null) return View(MVC.Errors.Views.Error);
            mainreport.RegBusinessObject("CommissionAllData", query.PRequestAllDataModels);
            mainreport.RegBusinessObject("Signers", null);
            mainreport.RegBusinessObject("MemberDetails", query.PMemberDetailsModel);
            mainreport.Load(Server.MapPath("~/Reports/ReportAllCommission.mrt"));

            mainreport.Render(true);
            mainreport.Compile();
            mainreport["NumberPrint"] = "1";
            mainreport["DatePrint"] = DateTime.Now;
            mainreport["LogoPrint"] = _reportsService.GetLogoUrl(new AddressUrlFile(Server.MapPath("~/")));

            //return StiMvcViewer.GetReportSnapshotResult(HttpContext, mainreport);
            return StiMvcViewerFx.GetReportSnapshotResult(mainreport);
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.CouncilAll, AllowInAccess = false)]
        public virtual ActionResult GetReportAllCouncil()
        {
            string _lanFile = Server.MapPath("~/Reports/fa.xml");
            if (System.IO.File.Exists(_lanFile))
                StiConfig.LoadLocalization(_lanFile);

            string searcvalue = (string)TempData["searcvalue"];
            SearchType searchtype = (SearchType)TempData["searchtype"];

            var query = _reportsService.PrintCouncilAll( searcvalue,  searchtype);

            if (query == null) return View(MVC.Errors.Views.Error);
            var mainreport = new StiReport();

            mainreport.RegBusinessObject("CommissionAllData", query.PRequestAllDataModels);
            mainreport.RegBusinessObject("Signers", null);
            mainreport.RegBusinessObject("MemberDetails", query.PMemberDetailsModel);
            mainreport.Load(Server.MapPath("~/Reports/ReportAllCouncil.mrt"));

            mainreport.Render(true);
            mainreport.Compile();
            mainreport["NumberPrint"] = "1";
            mainreport["DatePrint"] = DateTime.Now;
            mainreport["LogoPrint"] = _reportsService.GetLogoUrl(new AddressUrlFile(Server.MapPath("~/")));
            //return StiMvcViewer.GetReportSnapshotResult(HttpContext, mainreport);
            return StiMvcViewerFx.GetReportSnapshotResult(mainreport);
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.CommisionAll, AllowInAccess = false)]
        public virtual ActionResult GetAllCommission(string searcvalue, int searchTypeid)
        {
            var serchtype = (SearchType)(searchTypeid);
            var commissions = _commissionService.GetCommissionsProfileStudent(searcvalue, serchtype).Item3;
            var ja = new JArray();
            foreach (var itemObject in commissions.Select(item => new JObject
                {
                    {"Id",item.RequestId},
                    {"NameFamily",item.Request.Person.Profile.Name+" "+item.Request.Person.Profile.Family},
                    {"StudentNumber",item.Request.Person.Student.StudentNumber},
                    {"CommissionSpecialEducations",string.Join(",",item.CommissionSpecialEducations.Select(s=>s.SpecialEducation.Name).ToArray())},
                    {"VoteText",item.Request.Vote == null ? " " : item.Request.Vote.VoteText},
                    {"VoteStatus",item.Request.Vote == null ? " " : item.Request.Vote.VoteStatus.GetDescription()}
                }))
            {
                ja.Add(itemObject);
            }
            var jo = new JObject { { "total", commissions.Count }, { "rows", ja } };
            return Content(JsonConvert.SerializeObject(jo));
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.CouncilAll, AllowInAccess = false)]
        public virtual ActionResult GetAllCouncil(string searcvalue, int searchTypeid)
        {
            var serchtype = (SearchType)(searchTypeid);
            var councils = _councilService.GetCouncilsProfileStudent(searcvalue, serchtype).Item3;
            var ja = new JArray();
            foreach (var itemObject in councils.Select(item => new JObject
                {
                    {"Id",item.RequestId},
                    {"NameFamily",item.Request.Person.Profile.Name+" "+item.Request.Person.Profile.Family},
                    {"StudentNumber",item.Request.Person.Student.StudentNumber},
                    {"ProblemText",item.ProblemText},
                    {"VoteText",item.Request.Vote == null ? " " : item.Request.Vote.VoteText},
                    {"VoteStatus",item.Request.Vote == null ? " " : item.Request.Vote.VoteStatus.GetDescription()}
                }))
            {
                ja.Add(itemObject);
            }
            var jo = new JObject { { "total", councils.Count }, { "rows", ja } };
            return Content(JsonConvert.SerializeObject(jo));
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.PrintCouncil, AllowInAccess = false)]
        public virtual ActionResult GetReportCouncil()
        {
            string _lanFile = Server.MapPath("~/Reports/fa.xml");
            if (System.IO.File.Exists(_lanFile))
                StiConfig.LoadLocalization(_lanFile);
            var idrequest = (long)TempData["RequestId"];
            var query = _reportsService.PrintCouncil(idrequest, new AddressUrlFile(Server.MapPath("~/")));
            if (query == null) return View(MVC.Errors.Views.Error);
            var mainreport = new StiReport();

            mainreport.RegBusinessObject("RequestData", query.PRequestDataModel);
            mainreport.RegBusinessObject("Signers", query.PSignersModel);
            mainreport.RegBusinessObject("MemberDetails", query.PMemberDetailsModel.OrderBy(f => f.RowNumber).Take(6).ToList());
            mainreport.RegBusinessObject("StudentInformation", query.PStudentInformationModel);
            mainreport.Load(Server.MapPath("~/Reports/ReportCouncil.mrt"));

            mainreport.Render(true);
            mainreport.Compile();


            mainreport["NumberPrint"] = "1";
            mainreport["DatePrint"] = DateTime.Now;
            mainreport["LogoPrint"] = _reportsService.GetLogoUrl(new AddressUrlFile(Server.MapPath("~/")));

            //return StiMvcViewer.GetReportSnapshotResult(HttpContext, mainreport);
            return StiMvcViewerFx.GetReportSnapshotResult(mainreport);
        }
        public virtual ActionResult ExportReport()
        {
            return StiMvcViewerFx.ExportReportResult(this.HttpContext.Request);
            //return StiMvcViewerFxHelper.ExportReportResult(HttpContext.Request);
        }
        
        public virtual ActionResult ViewReport()
        {
            return View("ReportShow");
        }
        public virtual ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
        public virtual ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult(this.HttpContext);
        }
    }
}