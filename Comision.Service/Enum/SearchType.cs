using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Service.Enum
{
    /// <summary>
    ///  جستجوی براساس ایتم های زیر تعیین می گردد
    /// </summary>
    public enum SearchType
    {
        [Description("شماره دانشجویی")]
        StudentNumber = 0,

        [Description("نام و نام خانودادگی")]
        StudentNameFamili,

        [Description("شماره پرونده")]
        FileNumber,
    }
}
