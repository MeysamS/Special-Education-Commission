using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class FollowUpDetailsModel
    {
        public long PersonId { get; set; }
        public string NameFamily { get; set; }
        public string Avatar { get; set; }
        public long PostId { get; set; }
        public string PostName { get; set; }
        public string Signature { get; set; }
        public CartableStatus CartableStatus { get; set; }
        public CartableValidation CartableValidation { get; set; }
        public int RowNumber { get; set; }
        public string Description { get; set; }
    }
}
