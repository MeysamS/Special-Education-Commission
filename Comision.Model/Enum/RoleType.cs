using System.ComponentModel;

namespace Comision.Model.Enum
{
    /// <summary>
    /// <span dir="rtl">نوع نقش</span>
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// <span dir="rtl">مدیر سازمان مرکزی</span>
        /// </summary>
        [Description("مدیریت سازمان مرکزی")]
        AdminCentral=1,

        /// <summary>
        /// <span dir="rtl">مدیر واحد مرکز استان</span>
        /// </summary>
        [Description("مدیریت واحد مرکز استان")]
        AdminBranch,

        /// <summary>
        /// <span dir="rtl">مدیر دانشگاه</span>
        /// </summary>
        [Description("مدیریت دانشگاه")]
        AdminUniversity,

        /// <summary>
        /// <span dir="rtl">نامشخص</span>
        /// </summary>
        [Description("نامشخص")]
        None

    }//end RoleType
}