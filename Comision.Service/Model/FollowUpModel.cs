using System;
using System.Collections.Generic;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class FollowUpModel
    {
        public virtual long RequestId { get; set; }
        public virtual DateTime RequestDate { get; set; }
        public virtual long CommissionCouncilNumber { get; set; }
        public virtual DateTime CommissionCouncilDate { get; set; }
        public long PersonId { get; set; }
        public string NameFamily { get; set; }
        public string Avatar { get; set; }
        public long StudentNumber { get; set; }
        public string FieldofStudy { get; set; }
        public Grade Grade { get; set; }
        public virtual ICollection<FollowUpDetailsModel> FollowUpDetails { get; set; }
    }
}
