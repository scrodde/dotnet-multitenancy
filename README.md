# About

This is a sample project showing how to enable multi-tenancy for a ASP.NET Core GraphQL API. 

Tenants can be configured in `appsettings.json`. Use `ITenantProvider` to resolve tenants. Tenants are resolved from the incoming `X-Tenant-Id` or `Host` HTTP header.

To enable `Host` based tenant resolution append required aliases in `/etc/hosts`. Eg.
```
...
127.0.0.1 siteb
```

## Instructions

* Run the project
* Open the browser and navigate to `http://localhost:5000/ui/playground` / `http://siteb:5000/ui/playground` and experiment with the sample request below

Sample request and response

```graphql
query {
  currentTenant {
    id
    name
    host
  }
}
```

```json
{
  "data": {
    "currentTenant": {
      "id": "90c65c58-576a-4283-aa7f-661445910dd0",
      "name": "Site A",
      "host": "localhost"
    }
  },
  "extensions": {}
}
```