using Books.Domain.Authors;
using Books.Domain.Books;
using Microsoft.AspNetCore.OData;
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

        await Infrastructure.ServiceRegistrar.InitializeServices(app.Services);

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

        BuildODataModel(builder);
    }

    private static void BuildODataModel(WebApplicationBuilder builder)
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var authorEntity = modelBuilder.EntityType<Author>();
        authorEntity.HasKey(book => book.Id);
        authorEntity.Property(book => book.Birthday);
        authorEntity.Property(book => book.Created);
        authorEntity.Property(book => book.Firstname);
        authorEntity.Property(book => book.Lastname);
        authorEntity.Property(book => book.OrganizationId);
        authorEntity.Property(book => book.Updated);
        authorEntity.ContainsMany(author => author.Books);
        var authorEntitySet = modelBuilder.EntitySet<Author>("Authors");

        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(book => book.Id);
        bookEntity.Property(book => book.AuthorId);
        bookEntity.Property(book => book.Created);
        bookEntity.Property(book => book.DatePublished);
        bookEntity.Property(book => book.Title);
        bookEntity.Property(book => book.Updated);
        bookEntity.ContainsRequired(book => book.Author);

        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");

        builder.Services.AddControllers().AddOData(
            options => options
            .Select()
            .Filter()
            .OrderBy()
            .Expand()
            .Count()
            .SetMaxTop(100)
            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));
    }
}