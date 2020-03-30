using System;
using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// شورای آموزشی
    /// </summary>
    public class Council : Auditable
    {
        /// <summary>
        /// شماره شورای آموزشی
        /// </summary>
        public long CouncilNumber { get; set; }
        /// <summary>
        /// شماره ردیف
        /// </summary>
        public int RowNumber { get; set; }
        /// <summary>
        /// تاریخ
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// متن مشکل اموزشی
        /// </summary>
        public string ProblemText { get; set; }

        /// <summary>
        /// ایدی درخواست
        /// کلید اصلی و خارجی
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// شی درخواست
        /// </summary>
        public virtual Request Request { get; set; }

        /// <summary>
        /// سازنه اولیه
        /// </summary>
        public Council()
        { }
    }
}
