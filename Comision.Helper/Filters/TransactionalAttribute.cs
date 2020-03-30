using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Helper.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed  class TransactionalAttribute:Attribute
    {
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
        public TimeSpan? Timeout { get; set; }
    }
}
