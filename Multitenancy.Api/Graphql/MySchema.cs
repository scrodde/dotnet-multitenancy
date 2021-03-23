using System;
using GraphQL.Types;

namespace Multitenancy.Api.Graphql
{
    public class MySchema : Schema
    {
        public MySchema(IServiceProvider provider)
            : base(provider)
        {
            Query = (MyQuery) provider.GetService(typeof(MyQuery));
        }
    }
}