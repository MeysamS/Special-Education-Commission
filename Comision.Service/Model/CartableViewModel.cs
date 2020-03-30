using System.Collections.Generic;
using Comision.Model.Domain;
using Comision.Model.Domain.UserDomain;

namespace Comision.Service.Model
{
   public  class CartableViewModel
    {
        //public long Id { get; set; }
        //public long RequestId { get; set; }
        public virtual Request Request { get; set; }
        public virtual Commission Commission { get; set; }
        public virtual Council Council { get; set; }
        public virtual Person Person { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Student Student { get; set; }
        public virtual OrganizationStructureName OrganizationStructureName { get; set; }
        public virtual ICollection<Cartable> Cartables { get; set; }
        public virtual ICollection<Signer> Signers { get; set; }
        public Signer Signer { get; set; }
        public int LastCommissionSigner { get; set; }
        public int LastCouncilSigner { get; set; }
    }
}
