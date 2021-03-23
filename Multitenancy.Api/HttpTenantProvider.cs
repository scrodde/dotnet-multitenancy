using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Multitenancy.Api.Abstract;

namespace Multitenancy.Api
{
    public class HttpTenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITenantRepository _tenantRepository;

        public HttpTenantProvider(IHttpContextAccessor httpContextAccessor, ITenantRepository tenantRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantRepository = tenantRepository;
        }

        public async Task<ITenant> GetAsync(CancellationToken cancellationToken = default)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new ResolveTenantException("HttpContext unavailable");

            ITenant tenant = await TryLoadById(httpContext, cancellationToken) ??
                             await TryLoadByHost(httpContext, cancellationToken);

            if (tenant != null)
                return tenant;

            throw new ResolveTenantException("Unable to resolve tenant by id or hostname");
        }

        private async Task<ITenant> TryLoadById(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var idStr = httpContext.Request.Headers["X-Tenant-Id"].FirstOrDefault();
            var hasId = Guid.TryParse(idStr, out var id);
            if (!hasId)
                return null;

            return await _tenantRepository.LoadByIdAsync(id, cancellationToken);
        }

        private async Task<ITenant> TryLoadByHost(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var host = httpContext.Request.Host.Host;
            return await _tenantRepository.LoadByHostAsync(host, cancellationToken);
        }
    }
}