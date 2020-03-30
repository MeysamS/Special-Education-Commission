using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comision.Model.Domain.UserDomain;

namespace Comision.Service.Model
{
   public sealed class PasswordModel
    {
        public long PersonId { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور قبلی")]
        public string OldPasswordHash { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور جدید")]
        public string NewPasswordHash { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور جدید")]
        public string RepeatPasswordHash { get; set; }
        
        public PasswordModel() { }

       public PasswordModel(long personId)
       {
           PersonId = personId;
       }
        public PasswordModel(PasswordModel passwordModel)
        {
            if (passwordModel == null)
                return;

            OldPasswordHash = passwordModel.OldPasswordHash;
            NewPasswordHash = passwordModel.NewPasswordHash;
            RepeatPasswordHash = passwordModel.RepeatPasswordHash;
            PersonId = passwordModel.PersonId;
          
        }
    }
}
