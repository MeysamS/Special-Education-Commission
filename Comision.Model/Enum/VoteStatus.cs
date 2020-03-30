using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Model.Enum
{
   public enum VoteStatus
    {
        [Description("رای موقت")]
        Temporary = 1,

        [Description("رای قطعی")]
        Permanent = 2
    }
}
