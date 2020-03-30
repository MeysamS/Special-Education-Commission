using System;
using System.Collections.Generic;
using Comision.Model.Common;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// گروه یا مستر اعضای شورا یا کمیسون
    /// </summary>
   public class MemberMaster : AuditableEntity<long>
    {
        /// <summary>
        /// نام گروه یا مستر
        /// </summary>
        public string Name { get; set; }
       
        /// <summary>
        /// تاریخ
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// نشان میدهد که اعضای کمیسیون یا شورا در حال حاضر چه کسانی می باشند
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// ایدی دانشگاه
        /// </summary>
        public long UniversityId { get; set; }

        /// <summary>
        /// شی دانشگاه
        /// </summary>
        public virtual University University { get; set; }

        /// <summary>
        /// نوع امضا کنندگان(کمیسیون یا شورا)
        /// </summary>
        public RequestType RequestType { get; set; }

        /// <summary>
        /// لیست اعضای کمیسون یا شورا
        /// </summary>
        public virtual ICollection<MemberDetails> MemberDetails { get; set; }

        /// <summary>
        /// لیست درخواست
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public MemberMaster()
        {

        }
    }
}
