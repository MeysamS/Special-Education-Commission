using System.ComponentModel;

namespace Comision.Model.Enum
{
    /// <summary>
    /// داده نوع شمارشی وضعیت کمیسون و شورای آموزشی
    /// </summary>
    public enum CartableStatus
    {
        [Description("تایید شده")]
        Confirmed = 1,

        [Description("برگشت خورده")]
        Returned = 2,

        [Description("صدور رای")]
        Verdict = 3,

        [Description("موافقت با رای پرونده")]
        AgreetoVote=4,

        [Description("مخالفت با رای پرونده")]
        OppositiontoVote=5
    }
}
