using System;
using Comision.Model.Domain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
   public class MemberMasterModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// نام گروه یا مستر
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// تاریخ
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// ایدی دانشگاه
        /// </summary>
        public long UniversityId { get; set; }

        /// <summary>
        /// شی دانشگاه
        /// </summary>
        public  University University { get; set; }

        /// <summary>
        /// نوع امضا کنندگان(کمیسیون یا شورا)
        /// </summary>
        public RequestType RequestType { get; set; }
    }
}
