using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Multitenancy.Api.Abstract;
using Multitenancy.Api.Graphql;

namespace Multitenancy.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<ITenantProvider, HttpTenantProvider>()
                .AddTransient<ITenantRepository, TenantRepository>()
                .Configure<MultitenancyOptions>(Configuration.GetSection(nameof(MultitenancyOptions)));

            // Add GraphQL services and configure options
            services
                .AddHttpContextAccessor()
                .AddTransient<MyUserContext>()
                .AddSingleton<MySchema>()
                .AddGraphQL((options, provider) =>
                {
                    options.EnableMetrics = Environment.IsDevelopment();
                    var logger = provider.GetRequiredService<ILogger<Startup>>();
                    options.UnhandledExceptionDelegate = ctx =>
                        logger.LogError("{Error} occurred", ctx.OriginalException.Message);
                })
                .AddUserContextBuilder(context =>
                    (MyUserContext)context.RequestServices.GetService(typeof(MyUserContext)))
                // Add required services for GraphQL request/response de/serialization
                .AddSystemTextJson() // For .NET Core 3+
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = Environment.IsDevelopment())
                .AddWebSockets() // Add required services for web socket support
                .AddDataLoader() // Add required services for DataLoader support
                .AddGraphTypes(typeof(MySchema)); // Add all IGraphType implementors in assembly which ChatSchema exists

            services.AddLogging(b => b.AddConsole());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // this is required for websockets support
            app.UseWebSockets();

            // use websocket middleware for ChatSchema at default path /graphql
            app.UseGraphQLWebSockets<MySchema>();

            // use HTTP middleware for ChatSchema at default path /graphql
            app.UseGraphQL<MySchema>();

            // use GraphiQL middleware at default path /ui/graphiql with default options
            app.UseGraphQLGraphiQL();

            // use GraphQL Playground middleware at default path /ui/playground with default options
            app.UseGraphQLPlayground();

            // use Altair middleware at default path /ui/altair with default options
            app.UseGraphQLAltair();

            // use Voyager middleware at default path /ui/voyager with default options
            app.UseGraphQLVoyager();
        }
    }
}