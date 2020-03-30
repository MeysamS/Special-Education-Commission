using System.ComponentModel;

namespace Comision.Model.Enum
{
    /// <summary>
    /// داده نوع شمارشی مقطع تحصیلی
    /// </summary>
    public enum Grade
    {
        /// <summary>
        /// کاردانی
        /// </summary>
        [Description("کاردانی")]
        Technician = 1,

        /// <summary>
        /// کارشناسی
        /// </summary>
        [Description("کارشناسی")]
        Expert = 2,

        /// <summary>
        /// کارشناسی_ارشد
        /// </summary>
        [Description("کارشناسی ارشد")]
        MA = 3,

        /// <summary>
        /// دکتری
        /// </summary>
        [Description("دکتری")]
        PHD =4,

        /// <summary>
        /// دکتری_حرفه_ای
        /// </summary>
        [Description("دکتری حرفه ای")]
        DVM = 5
    }
}
