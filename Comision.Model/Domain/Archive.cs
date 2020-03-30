using System;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// کلاس بایگانی
    /// </summary>
    public class Archive : AuditableEntity<long>
    {
        /// <summary>
        /// <span dir="rtl">کلید اصلی و کلید خارجی</span>
        /// </summary>
        [Index("IX_Archive", IsUnique = true)]
        public long RequestId { get; set; }

        /// <summary>
        /// شی درخواست
        /// </summary>
        public virtual Request Request { get; set; }

        /// <summary>
        /// <span dir="rtl">پرسنلی که درخواست را بایگانی کرده است</span>
        /// </summary>
        public long PersonId { get; set; }
        public virtual Person Person { get; set; }

        /// <summary>
        /// تاریخ بایگانی
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public Archive()
        { }
    }
}
