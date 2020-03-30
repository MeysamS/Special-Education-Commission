using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// موارد خاص آموزشی کمیسون
    /// </summary>
  public class CommissionSpecialEducation : Auditable
    {
        /// <summary>
        /// ایدی کمیسیون
        /// </summary>
        public long CommissionId { get; set; }

        /// <summary>
        /// ایدی مورد خاص آموزشی
        /// </summary>
        public long SpecialEducationId { get; set; }

        /// <summary>
        ///شی کمیسیون
        /// </summary>
        public virtual Commission Commission{ get; set; }
       
        /// <summary>
        /// شی مورد خاص اموزشی
        /// </summary>
        public virtual SpecialEducation SpecialEducation { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public CommissionSpecialEducation()
        { }
    }
}
