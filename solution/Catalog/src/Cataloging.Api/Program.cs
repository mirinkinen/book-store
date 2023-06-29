using Cataloging.Api.Auditing;
using Cataloging.Application.Requests.Books.GetBooks;
using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.ModelBuilder;
using Shared.Application.Auditing;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Cataloging.Api;

[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Must not be static for API tests.")]
public class Program
{
    public static async Task Main(string[] args)
    {
        IEnumerable<string> strings = Enumerable.Empty<string>();

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseWolverine(opts =>
        {
            opts.ApplicationAssembly = typeof(GetBooksHandler).Assembly;
            opts.Policies.AddMiddleware(typeof(AuditableQueryMiddleware), filter => typeof(IAuditableQuery).IsAssignableFrom(filter.MessageType));
            opts.Policies.AddMiddleware(typeof(AuditableCommandMiddleware), filter => typeof(IAuditableCommand).IsAssignableFrom(filter.MessageType));
            opts.Policies.LogMessageStarting(LogLevel.Debug);
        });

        AddApiServices(builder);
        Application.ServiceRegistrar.RegisterApplicationServices(builder.Services);
        Infrastructure.ServiceRegistrar.RegisterInfrastructureServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuditLogging();
        app.MapControllers();

        await app.RunAsync();
    }

    private static void AddApiServices(WebApplicationBuilder builder)
    {
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