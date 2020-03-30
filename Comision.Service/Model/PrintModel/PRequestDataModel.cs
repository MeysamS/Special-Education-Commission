using System;

namespace Comision.Service.Model.PrintModel
{
    /// <summary>
    /// مدل درخواست
    /// </summary>
    public class PRequestDataModel
    {
        public long RequestId { get; set; }
        public long CommissionCouncilNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string VoteText { get; set; }
        public string DescriptionVerdict { get; set; }
        public string ProblemText { get; set; }
        public int RowNumber { get; set; }

        /// <summary>
        /// متن معاون آموزشی
        /// </summary>
        public string RefertoText { get; set; }

    }
}
