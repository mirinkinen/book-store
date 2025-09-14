using Infra.Data;

namespace API;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.Services.RegisterServices(builder.Configuration);
        
        return builder;
    }
}