using System.ComponentModel;

namespace Comision.Model.Enum
{
    public enum CartableValidation
    {
        /// <summary>
        /// معتبر
        /// </summary>
        [Description("معتبر")]
        Valid = 1,

        /// <summary>
        /// نا معتبر
        /// </summary>
        [Description("نا معتبر")]
        Invalid,

        /// <summary>
        /// نا مشخص
        /// </summary>
        [Description("نا مشخص")]
        None
    }
}
