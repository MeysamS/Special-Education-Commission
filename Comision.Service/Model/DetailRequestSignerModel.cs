namespace Comision.Service.Model
{
    public class DetailRequestSignerModel
    {
        public long PostId { get; set; }
        public string PostName { get; set; }
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        public int RowNumber { get; set; }
        public bool Signed { get; set; }
        public string Signature { get; set; }

    }
}
