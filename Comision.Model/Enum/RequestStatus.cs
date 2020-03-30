using System.ComponentModel;

namespace Comision.Model.Enum
{
    public enum RequestStatus
    {
        [Description("در انتظار")]
        Waiting = 1,

        [Description("در جریان")]
        InFlow=2,

        [Description("برگشت خورده")]
        Returned=3,

        /// <summary>
        /// زمانی که درخواست کمیسیون باشد و به مرحله بعد نیاز نداشته باشد این مقدار را می گیرد
        /// </summary>
        [Description("صدور رای")]
        Verdict = 4,
     
        ///// <summary>
        ///// رای مثبت صادر شد
        ///// </summary>
        //[Description("رای موافق")]
        //FavorVote,

        ///// <summary>
        ///// رای منفی صادر شد
        ///// </summary>
        //[Description("رای مخالف")]
        //Veto,

        [Description("موافقت با رای پرونده")]
        AgreetoVote=5,

        [Description("مخالفت با رای پرونده")]
        OppositiontoVote=6,

        [Description("بایگانی")]
        Archive=7
    }
}
