using System.ComponentModel;

namespace Comision.Model.Enum
{
    /// <summary>
    /// داده نوع شمارشی احراز هویت
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// مدیریت مرکز
        /// </summary>
        [Description("مدیریت سازمان مرکزی")]
        AdminCentral = 1,

        /// <summary>
        /// مدیریت واحد استاد
        /// </summary>
        [Description("مدیریت واحد استان")]
        AdminBranch = 2,

        /// <summary>
        /// مدیریت دانشگاه
        /// </summary>
        [Description("مدیریت دانشگاه")]
        AdminUniversity = 3,

        /// <summary>
        /// پرسنل
        /// </summary>
        [Description("پرسنل")]
        Personel = 4,

        /// <summary>
        /// دانشجو
        /// </summary>
        [Description("دانشجو")]
        Student = 5,

        /// <summary>
        /// <span dir="rtl">نامشخص</span>
        /// </summary>
        [Description("نامشخص")]
        None
    }
}
