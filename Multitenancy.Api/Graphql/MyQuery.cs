using GraphQL.Types;
using Multitenancy.Api.Graphql.Types;

namespace Multitenancy.Api.Graphql
{
    public class MyQuery : ObjectGraphType
    {
        public MyQuery()
        {
            Field<TenantType>("currentTenant", resolve: _ =>
            {
                var ctx = (MyUserContext) _.UserContext;
                return ctx.TenantProvider.GetAsync();
            });
        }
    }
}