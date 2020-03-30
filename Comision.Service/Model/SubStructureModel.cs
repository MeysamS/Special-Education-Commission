using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class SubStructureModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public StructureType StructureType { get; set; }
    }
}
