using System.ComponentModel;

namespace Comision.Model.Enum
{
    /// <summary>
    /// وضعیت خدمت سربازی
    /// </summary>
    public enum MilitaryServiceStatus
    {

        /// <summary>
        /// معاف
        /// </summary>
        [Description("معاف")]
        Exempt = 1,        

        /// <summary>
        /// مشمول
        /// </summary>
        [Description("مشمول")]
        Included = 2,

        /// <summary>
        /// در حال خدمت
        /// </summary>
        [Description("در حال خدمت")]
        Serving = 3,

        /// <summary>
        /// دارای کارت پایان خدمت
        /// </summary>
        [Description("دارای کارت پایان خدمت")]
        CardService = 4,

        /// <summary>
        /// هیچکدام برای جنسیت خانم
        /// </summary>
        [Description("هیچکدام")]
        None = 5
    }
}
