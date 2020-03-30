using System.ComponentModel;

namespace Comision.Model.Enum
{

    /// <summary>
    /// نوع متن های پیش فرض
    /// </summary>
    public enum TextDefaultType : int
    {

        [Description("متن پیش فرض صدور رای کمیسیون")]
        VoteCommissionText = 1,

        [Description("متن پیش فرض صدور رای شورا")]
        VoteCouncilText = 2,

        [Description("متن پیش فرض مشکل آموزشی")]
        ProblemText = 3,

        [Description("متن پیش فرض ارجاعات")]
        SpecialEducationVerdictText = 4

    }
}
