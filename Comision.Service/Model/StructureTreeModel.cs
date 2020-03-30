using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class StructureTreeModel
    {
        public long? Id { get; set; }

        //[Required(ErrorMessageResourceType =typeof(Resources.Resources.Resources) ,ErrorMessageResourceName = "Required")]
        public string Text { get; set; }

        public long? OrgId { get; set; }
        public bool? HasChild { get; set; }

        public long ParentId { get; set; }
        public StructureType StructureType { get; set; }

    }
}
