using Infra.Data;

namespace API;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.ConfigureGraphQL();
        builder.Services.RegisterServices(builder.Configuration);
        
        return builder;
    }
    
    internal static void ConfigureGraphQL(this WebApplicationBuilder builder)
    {
        builder.AddGraphQL()
            .AddGraphQLServer()
            .AddQueryType()
            .AddMutationType()
            .AddTypes()
            .RegisterDbContextFactory<CatalogDbContext>();
    }
}