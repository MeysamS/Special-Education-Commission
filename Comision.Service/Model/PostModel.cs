using System.ComponentModel.DataAnnotations;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
   public class PostModel
    {
        /// <summary>
        /// ایدی پست
        /// </summary>
        public long? Id { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = @"عنوان")]
        public string Name { get; set; }

        ///[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = @"سطح سمت")]
        public PostType PostType { get; set; }

    }
}
