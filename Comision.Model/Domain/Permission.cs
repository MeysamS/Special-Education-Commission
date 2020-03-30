using System.Xml.Serialization;
using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// سطح دستری ایتم ها
    /// </summary>
    [XmlRoot("Permission")]
    public class Permission: AuditableEntity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [XmlElement(ElementName = "Area")]
        public string Area { get; set; }

        [XmlElement(ElementName = "Control")]
        public string Control { get; set; }

        //public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// سازنده اولیه
        /// </summary>
        public Permission()
        {

        }
    }
}
