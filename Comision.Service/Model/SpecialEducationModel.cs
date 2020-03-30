using System.ComponentModel.DataAnnotations;

namespace Comision.Service.Model
{
    public class SpecialEducationModel
    {
        public long? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
    }
}
