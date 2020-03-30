using Comision.Model.Common;

namespace Comision.Model.Domain
{
    /// <summary>
    /// پیوست های درخواست کمسیون و شورا
    /// </summary>
    public class Attachment : AuditableEntity<long>
    {
        /// <summary>
        /// عنوان فایل پیوست
        /// </summary>
        public string Title { get; set; }
        public long RequestId { get; set; }
        public virtual Request Request { get; set; }

        /// <summary>
        /// آدرس فایل پیوست
        /// </summary>
        public string File { get; set; }
        public string Extention { get; set; }
        public long Size { get; set; }
    }
}
