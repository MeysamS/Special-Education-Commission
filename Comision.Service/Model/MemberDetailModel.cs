namespace Comision.Service.Model
{
   public class MemberDetailModel
    {
        public long? Id { get; set; }

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
        public int RowNumber { get; set; }
        /// <summary>
        ///ایدی گروه یا مستر
        /// </summary>
        public long MemberMasterId { get; set; }

  
    }
}
