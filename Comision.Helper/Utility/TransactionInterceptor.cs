using Comision.Helper.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Comision.Data.Context
{
   public class TransactionInterceptor: ISyncInterceptionBehavior
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IMethodInvocationResult Intercept(ISyncMethodInvocation methodInvocation)
        {
            var transactionAttribute = GetTransactionaAttributeOrNull(methodInvocation.InstanceMethodInfo);

            if (transactionAttribute == null || _unitOfWork.HasTransaction) return methodInvocation.InvokeNext();

            using (var transaction = _unitOfWork.BeginTransaction(transactionAttribute.IsolationLevel))
            {
                var result = methodInvocation.InvokeNext();

                if (result.Successful)
                    transaction.Commit();
                else
                {
                    transaction.Rollback();
                }

                return result;
            }
        }

        private static TransactionalAttribute GetTransactionaAttributeOrNull(MemberInfo methodInfo)
        {
            var transactionalAttribute =
                ReflectionHelper.GetAttributesOfMemberAndDeclaringType<TransactionalAttribute>(
                    methodInfo
                ).FirstOrDefault();

            return transactionalAttribute;
        }
    }
}
