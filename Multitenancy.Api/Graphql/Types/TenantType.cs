using GraphQL.Types;
using Multitenancy.Api.Abstract;

namespace Multitenancy.Api.Graphql.Types
{
    public class TenantType : AutoRegisteringObjectGraphType<ITenant>
    {
    }
}