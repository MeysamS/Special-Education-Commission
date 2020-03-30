using Comision.Model.Enum;

namespace Comision.Service.Model
{
   public class PostPersonModel
    {
        public long PersonId { get; set; }
        public long PostId { get; set; }
        public string PostName { get; set; }
        public long LevelId { get; set; }
        public string LevelName { get; set; }
        public PostType PostType { get; set; }
    }
}
