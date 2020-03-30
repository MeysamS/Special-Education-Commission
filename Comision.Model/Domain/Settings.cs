using Comision.Model.Common;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
   public class Settings :  Auditable
    {
        /// <summary>
        /// ایدی دانشگاه
        /// </summary>
        public long UniversityId { get; set; }
        public University University { get; set; }
        public string SmsUserName { get; set; }
        public string SmsPass { get; set; }
        public string SmsNumber { get; set; }
        public string TextSms { get; set; }
        public string SmtpHost { get; set; }

        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "EmailInvalid")]
        public string SmtpFrom { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPass { get; set; }
        public string SmtpPort { get; set; }
        public Ordinal Ordinal { get; set; }

        /// <summary>
        /// بازه شماره کمیسیون
        /// </summary>
        public long CommissionNumberRepetitions { get; set; }

        /// <summary>
        /// بازه شماره شورا
        /// </summary>
        public long CouncilNumberRepetitions { get; set; }
    }
}
