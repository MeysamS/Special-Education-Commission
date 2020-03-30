using System.Collections.Generic;
using Comision.Model.Domain;

namespace Comision.Service.ImplementationService
{
    public class DistinctItemComparer : IEqualityComparer<FieldofStudy>
    {
        public bool Equals(FieldofStudy x, FieldofStudy y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(FieldofStudy obj)
        {
            return obj.Id.GetHashCode(); 
            //^obj.Name.GetHashCode() ^obj.Code.GetHashCode() ^obj.Price.GetHashCode();
        }
    }
}
