using System;

namespace Multitenancy.Api.Abstract
{
    public interface ITenant
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Host { get; }
    }
}