using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// <span dir="rtl">راي</span>
    /// </summary>
    public class Vote : Auditable
    {
        /// <summary>
        /// <span dir="rtl">کلید اصلی و کلید خارجی</span>
        /// </summary>
        [Index("IX_RequestId", IsUnique = true)]
        public long RequestId { get; set; }
        /// <summary>
        /// ایدی پرسنل
        /// </summary>
        public long PersonId { get; set; }
        /// <summary>
        ///ایدی سمت شخص
        /// </summary>
        public long PostId { get; set; }

        /// <summary>
        /// تاریخ صدور رای
        /// </summary>
        [UIHint("PersianDatePicker")]
        public DateTime DateVote { get; set; }
        /// <summary>
        /// شی درخواست
        /// </summary>
        public virtual Request Request { get; set; }
        /// <summary>
        /// شی شخص
        /// </summary>
        public virtual Person Person { get; set; }
        public virtual VoteType VoteType { get; set; }
        public virtual VoteStatus VoteStatus { get; set; }
        /// <summary>
        /// شی سمت
        /// </summary>   
        public virtual Post Post { get; set; }
        /// <summary>
        /// متن رای
        /// </summary>    
        public string VoteText { get; set; }

        /// <summary>
        /// متن ارجاعات
        /// </summary>    
        public string ReferText { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public Vote() { }

    }//end Vote
}