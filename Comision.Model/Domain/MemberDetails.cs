using System.ComponentModel.DataAnnotations.Schema;
using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// جزئیات اعضای شورا و کمیسون 
    /// </summary>
    public class MemberDetails : AuditableEntity<long>
    {
        /// <summary>
        ///ایدی گروه یا مستر
        /// </summary>
        [Index("IX_RowNumber", IsUnique = true, Order = 1)]
        public long MemberMasterId { get; set; }

        /// <summary>
        /// شی شخص
        /// </summary>
        //public Person Person { get; set; }

        /// <summary>
        /// ایدی شخص
        /// </summary>
        //public long PersonId { get; set; }

        /// <summary>
        /// شخص
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// سمت
        /// </summary>
        public string PostName { get; set; }

        /// <summary>
        /// شماره ردیف
        /// </summary>
        [Index("IX_RowNumber", IsUnique = true, Order = 2)]
        public int RowNumber { get; set; }

        /// <summary>
        ///شی گروه یا مستر
        /// </summary>
        public virtual MemberMaster MemberMaster { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public MemberDetails()
        { }
    }
}
