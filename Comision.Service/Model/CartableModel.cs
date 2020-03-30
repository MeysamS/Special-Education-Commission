using System;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class CartableModel
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public virtual Request Request { get; set; }
        public virtual Commission Commission { get; set; }
        public virtual Council Council { get; set; }
        public virtual Person Person { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Student Student { get; set; }
        public virtual Personel Personel { get; set; }
        public long PersonId { get; set; }
        public long PostId { get; set; }
        public virtual Post Post { get; set; }
        public CartableStatus CartableStatus { get; set; }
        public short RowNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
    }
}
