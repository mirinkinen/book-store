using Books.Api.OData.Serialization;
using Books.Domain.Authors;
using Books.Domain.Books;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.ModelBuilder;
using System.Diagnostics.CodeAnalysis;

namespace Books.Api;

[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Must not be static for API tests.")]
public class Program
{
    public static async Task Main(string[] args)
    {
        IEnumerable<string> strings = Enumerable.Empty<string>();

        var builder = WebApplication.CreateBuilder(args);

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
        bookEntity.Property(book => book.Title);
        bookEntity.Property(book => book.ModifiedAt);
        bookEntity.Property(book => book.ModifiedBy);
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
                // Add serializers as singletons, since they have no state.
                services.AddSingleton<ODataCollectionSerializer, CustomODataCollectionSerializer>();
                services.AddSingleton<ODataDeltaResourceSetSerializer, CustomODataDeltaResourceSetSerializer>();
                services.AddSingleton<ODataEntityReferenceLinkSerializer, CustomODataEntityReferenceLinkSerializer>();
                services.AddSingleton<ODataEntityReferenceLinksSerializer, CustomODataEntityReferenceLinksSerializer>();
                services.AddSingleton<ODataEnumSerializer, CustomODataEnumSerializer>();
                services.AddSingleton<ODataErrorSerializer, CustomODataErrorSerializer>();
                services.AddSingleton<ODataMetadataSerializer, CustomODataMetadataSerializer>();
                services.AddSingleton<ODataPrimitiveSerializer, CustomODataPrimitiveSerializer>();
                services.AddSingleton<ODataRawValueSerializer, CustomODataRawValueSerializer>();
                services.AddSingleton<ODataResourceSerializer, CustomODataResourceSerializer>();
                services.AddSingleton<ODataServiceDocumentSerializer, CustomODataServiceDocumentSerializer>();

                services.AddHttpContextAccessor();
            }));
    }
}