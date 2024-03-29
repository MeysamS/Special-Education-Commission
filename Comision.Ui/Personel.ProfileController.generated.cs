// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Comision.Ui.Areas.Personel.Controllers
{
    public partial class ProfileController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ProfileController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SaveProfile()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveProfile);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SavePassword()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SavePassword);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DeleteAvatar()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteAvatar);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProfileController Actions { get { return MVC.Personel.Profile; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Personel";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Profile";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Profile";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string SaveProfile = "SaveProfile";
            public readonly string SavePassword = "SavePassword";
            public readonly string DeleteAvatar = "DeleteAvatar";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string SaveProfile = "SaveProfile";
            public const string SavePassword = "SavePassword";
            public const string DeleteAvatar = "DeleteAvatar";
        }


        static readonly ActionParamsClass_SaveProfile s_params_SaveProfile = new ActionParamsClass_SaveProfile();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveProfile SaveProfileParams { get { return s_params_SaveProfile; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveProfile
        {
            public readonly string profileModel = "profileModel";
            public readonly string avatarFile = "avatarFile";
        }
        static readonly ActionParamsClass_SavePassword s_params_SavePassword = new ActionParamsClass_SavePassword();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SavePassword SavePasswordParams { get { return s_params_SavePassword; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SavePassword
        {
            public readonly string passwordModel = "passwordModel";
        }
        static readonly ActionParamsClass_DeleteAvatar s_params_DeleteAvatar = new ActionParamsClass_DeleteAvatar();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteAvatar DeleteAvatarParams { get { return s_params_DeleteAvatar; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteAvatar
        {
            public readonly string profileId = "profileId";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _Personel = "_Personel";
                public readonly string _Profile = "_Profile";
                public readonly string _User = "_User";
                public readonly string Index = "Index";
            }
            public readonly string _Personel = "~/Areas/Personel/Views/Profile/_Personel.cshtml";
            public readonly string _Profile = "~/Areas/Personel/Views/Profile/_Profile.cshtml";
            public readonly string _User = "~/Areas/Personel/Views/Profile/_User.cshtml";
            public readonly string Index = "~/Areas/Personel/Views/Profile/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ProfileController : Comision.Ui.Areas.Personel.Controllers.ProfileController
    {
        public T4MVC_ProfileController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SaveProfileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Comision.Service.Model.ProfileModel profileModel, System.Web.HttpPostedFileBase avatarFile);

        [NonAction]
        public override System.Web.Mvc.ActionResult SaveProfile(Comision.Service.Model.ProfileModel profileModel, System.Web.HttpPostedFileBase avatarFile)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveProfile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "profileModel", profileModel);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "avatarFile", avatarFile);
            SaveProfileOverride(callInfo, profileModel, avatarFile);
            return callInfo;
        }

        [NonAction]
        partial void SavePasswordOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Comision.Service.Model.PasswordModel passwordModel);

        [NonAction]
        public override System.Web.Mvc.ActionResult SavePassword(Comision.Service.Model.PasswordModel passwordModel)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SavePassword);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "passwordModel", passwordModel);
            SavePasswordOverride(callInfo, passwordModel);
            return callInfo;
        }

        [NonAction]
        partial void DeleteAvatarOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long profileId);

        [NonAction]
        public override System.Web.Mvc.ActionResult DeleteAvatar(long profileId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteAvatar);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "profileId", profileId);
            DeleteAvatarOverride(callInfo, profileId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
