using Cataloging.Api.Auditing;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.ModelBuilder;
using Shared.Application.Auditing;
using Wolverine;

namespace Cataloging.Api;

public static class ServiceRegistrar
{
    internal static void RegisterApiServices(WebApplicationBuilder builder)
    {
        builder.Host.UseWolverine(opts =>
        {
            opts.Discovery.IncludeAssembly(typeof(GetBooksHandler).Assembly);
            opts.Policies.AddMiddleware(typeof(AuditableQueryMiddleware), filter => typeof(IAuditableQuery).IsAssignableFrom(filter.MessageType));
            opts.Policies.AddMiddleware(typeof(AuditableCommandMiddleware), filter => typeof(IAuditableCommand).IsAssignableFrom(filter.MessageType));
            opts.Policies.LogMessageStarting(LogLevel.Debug);
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
                services.AddSingleton<ODataResourceSerializer, AuditingODataResourceSerializer>();

                services.AddHttpContextAccessor();
            }));
    }
}