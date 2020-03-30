using System.ComponentModel.DataAnnotations;
using Comision.Model.Enum;

namespace Comision.Ui.Areas.Admin.Models
{
    public class TextDefaultViewModel
    {
        public long? Id { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = @"متن")]
        public string Text { get; set; }

        [Display(Name = @"نوع متن")]
        public TextDefaultType TextDefaultType { get; set; }

    }
}