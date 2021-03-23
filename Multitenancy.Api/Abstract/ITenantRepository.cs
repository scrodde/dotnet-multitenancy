using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multitenancy.Api.Abstract
{
    public interface ITenantRepository
    {
        Task<ITenant> LoadByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ITenant> LoadByHostAsync(string host, CancellationToken cancellationToken = default);
    }
}