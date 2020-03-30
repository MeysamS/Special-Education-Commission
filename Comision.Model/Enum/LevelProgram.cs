using System.ComponentModel;

namespace Comision.Model.Enum
{
    public enum LevelProgram
    {
        /// <summary>
        /// <span dir="rtl">سازمان مرکزی</span>
        /// </summary>
        [Description("سازمان مرکزی")]
        CentralOrganization = 1,

        /// <summary>
        /// <span dir="rtl">واحد مرکز استان</span>
        /// </summary>
        [Description("واحد مرکز استان")]
        BranchProvince,

        /// <summary>
        /// <span dir="rtl">دانشگاه</span>
        /// </summary>
        [Description("دانشگاه")]
        University
    }
}
