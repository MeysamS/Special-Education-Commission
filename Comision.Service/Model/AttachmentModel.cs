using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Service.Model
{
    public class AttachmentModel
    {
        /// <summary>
        /// عنوان فایل پیوست
        /// </summary>
        public string Title { get; set; }

        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        public long RequestId { get; set; }
        
        /// <summary>
        /// آدرس فایل پیوست
        /// </summary>
        public string File { get; set; }
        public string Extention { get; set; }
        public long Size { get; set; }
    }
}
