using System.ComponentModel;

namespace Comision.Model.Enum
{
    public enum VoteType
    {
        [Description("رای موافق")]
        FavorVote = 1,

        [Description("رای مخالف")]
        Veto = 2
    }
}
