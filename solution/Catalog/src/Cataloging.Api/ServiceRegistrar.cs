using Cataloging.Application.Requests.Authors;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using Cataloging.Infrastructure.Database.Setup;
using Common.Api.Auditing;
using Common.Application.Auditing;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.ModelBuilder;
using Oakton;
using Oakton.Resources;
using Wolverine;

namespace Cataloging.Api;

public static class ServiceRegistrar
{
    internal static void RegisterApiServices(WebApplicationBuilder builder, string connectionString)
    {
        builder.Host.ApplyOaktonExtensions();
        builder.Services.AddScoped<IStatefulResource, DatabaseInitializer>();

        // All commands are handled by Wolverine.
        builder.Host.UseWolverine(opts =>
        {
            opts.Discovery.IncludeAssembly(typeof(GetBooksHandler).Assembly);
            opts.Discovery.IncludeAssembly(typeof(AuditLogEventHandler).Assembly);

            opts.Policies.ForMessagesOfType<IAuthorCommand>().AddMiddleware(typeof(LoadAuthorMiddleware));
            opts.Policies.LogMessageStarting(LogLevel.Debug);

            Infrastructure.ServiceRegistrar.UseWolverine(opts, connectionString);

            //opts.CodeGeneration.TypeLoadMode = JasperFx.CodeGeneration.TypeLoadMode.Auto;
        });


        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpContextAccessor();

        builder.Services.Configure<AuditOptions>(builder.Configuration.GetSection(AuditOptions.Audit));

        AddOData(builder);
    }

    private static void AddOData(WebApplicationBuilder builder)
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var authorEntity = modelBuilder.EntityType<Author>();
        authorEntity.HasKey(book => book.Id);
        authorEntity.Property(book => book.Birthday);
        authorEntity.Property(book => book.CreatedAt);
        authorEntity.Property(book => book.FirstName);
        authorEntity.Property(book => book.LastName);
        authorEntity.Property(book => book.OrganizationId);
        authorEntity.Property(book => book.ModifiedAt);
        authorEntity.Property(book => book.ModifiedBy);
        authorEntity.ContainsMany(author => author.Books);
        var authorEntitySet = modelBuilder.EntitySet<Author>("Authors");

        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(book => book.Id);
        bookEntity.Property(book => book.AuthorId);
        bookEntity.Property(book => book.CreatedAt);
        bookEntity.Property(book => book.DatePublished);
        bookEntity.Property(book => book.ModifiedAt);
        bookEntity.Property(book => book.ModifiedBy);
        bookEntity.Property(book => book.Price);
        bookEntity.Property(book => book.Title);
        bookEntity.ContainsRequired(book => book.Author);

        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");

        builder.Services.AddControllers().AddOData(
            options => options
                .Count()
                .Expand()
                .Filter()
                .OrderBy()
                .Select()
                .SetMaxTop(20)
                .AddRouteComponents("v1", modelBuilder.GetEdmModel(), services =>
                {
                    // OData seems to have its own container.
                    // Register services here that are used in overridden OData implementations.
                    services.AddSingleton<ODataResourceSerializer, AuditingODataResourceSerializer>();
                    services.AddHttpContextAccessor();
                }));
    }
}