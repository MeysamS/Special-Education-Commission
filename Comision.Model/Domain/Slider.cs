using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Common;

namespace Comision.Model.Domain
{
  public  class Slider:Entity<int>
  {

        [Display(Name = "تصویر")]
        [DisplayName("تصویر")]
        public string ImageName { get; set; }
    }
}
