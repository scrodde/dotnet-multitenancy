using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Multitenancy.Api.Abstract;

namespace Multitenancy.Api
{
    public class TenantRepository : ITenantRepository
    {
        private readonly MultitenancyOptions _options;

        public TenantRepository(IOptions<MultitenancyOptions> options)
        {
            _options = options.Value;
        }

        public Task<ITenant> LoadByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tenant = _options.Tenants.FirstOrDefault(t => t.Id == id);
            return Task.FromResult<ITenant>(tenant);
        }

        public Task<ITenant> LoadByHostAsync(string host, CancellationToken cancellationToken = default)
        {
            var tenant = _options.Tenants.FirstOrDefault(t =>
                String.Equals(t.Host, host, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult<ITenant>(tenant);
        }
    }

    public class MultitenancyOptions
    {
        public List<TenantOptions> Tenants { get; set; }

        public class TenantOptions : ITenant
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Host { get; set; }
        }
    }
}