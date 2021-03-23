using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multitenancy.Api.Abstract
{
    public interface ITenantProvider
    {
        Task<ITenant> GetAsync(CancellationToken cancellationToken = default);
    }

    public class ResolveTenantException : ApplicationException
    {
        public ResolveTenantException(string msg)
            : base(msg)
        {
        }
    }
}