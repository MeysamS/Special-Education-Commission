using Comision.Model.Common;
using Comision.Model.Enum;

namespace Comision.Model.Domain
{
    /// <summary>
    /// کلاس متن های پیش فرض
    /// </summary>
    public class TextDefault: AuditableEntity<long>
    {
        public string Text { get; set; }

        public TextDefaultType TextDefaultType { get; set; }

        public long UnivercityId { get; set; }

        public virtual University University { get; set; }
    }
}
