using Comision.Model.Domain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class SignerModel
    {
        public long? Id { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        public long PostId { get; set; }
        public virtual Post Post { get; set; }
        public RequestType RequestType { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        public int RowNumber { get; set; }
    }
}
