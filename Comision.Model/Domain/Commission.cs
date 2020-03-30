using System;
using System.Collections.Generic;
using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// کمیسون
    /// </summary>
    public class Commission : Auditable
    {
        /// <summary>
        /// شماره کمیسیون
        /// </summary>
        public long CommissionNumber { get; set; }
        /// <summary>
        /// شماره ردیف
        /// </summary>
        public int RowNumber { get; set; }
        /// <summary>
        /// تاریخ کمیسیون
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
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
        /// لیست موارد خاص مربوط به کمیسون
        /// </summary>
        public virtual ICollection<CommissionSpecialEducation> CommissionSpecialEducations { get; set; }
        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public Commission()
        { }
    }
}
