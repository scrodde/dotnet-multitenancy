using System.Collections.Generic;
using Multitenancy.Api.Abstract;

namespace Multitenancy.Api.Graphql
{
    public class MyUserContext : Dictionary<string, object>
    {
        public MyUserContext(ITenantProvider tenantProvider)
        {
            TenantProvider = tenantProvider;
        }

        public ITenantProvider TenantProvider { get; }
    }
}