using System;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
  public  class VoteModel
    {
        /// <summary>
        /// <span dir="rtl">کلید اصلی و کلید خارجی</span>
        /// </summary>
        public long RequestId { get; set; }
        public RequestType RequestType { get; set; }
        public bool Included { get; set; }
        /// <summary>
        /// ایدی پرسنل
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// ایدی پرسنل از طرف
        /// </summary>
        public long PersonIdOnBehalfof { get; set; }

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
        /// متن رای
        /// </summary>    
        public string VoteText { get; set; }

        /// <summary>
        /// متن ارجاعات
        /// </summary>    
        public string VoteDescription { get; set; }

        /// <summary>
        /// شماره مرحله
        /// </summary>    
        public int RowNumber { get; set; }
        public bool ReferTo { get; set; }
        /// <summary>
        /// اطلاعات دانشجو
        /// </summary>  
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public long StudentNumber { get; set; }
        public string FieldofStudy { get; set; }
        public long FieldofStudyId { get; set; }
        public string Grade { get; set; }
        public VoteType VoteType { get; set; }
        public VoteStatus VoteStatus { get; set; }
        public DetailRequestModel DetailRequestModel { get; set; }
    }
}
