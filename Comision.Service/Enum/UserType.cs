using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Service.Enum
{
    public enum UserType
    {
        [Description("پرسنل")]
        Personel = 0,

        [Description("دانشجو")]
        Student,

        [Description("همه کاربران")]
        All
    }
}
