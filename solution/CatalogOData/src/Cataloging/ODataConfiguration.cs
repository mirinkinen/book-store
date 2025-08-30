using Cataloging.API;
using Cataloging.Domain;
using JasperFx.Core;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Cataloging;

internal static class ODataConfiguration
{
    internal static void AddOData(this WebApplicationBuilder builder)
    {
        var edmModelV1 = GetEdmModelV1();
        var edmModelV2 = GetEdmModelV2();
        var edmModelV3 = GetEdmModelV3();

        builder.Services.AddControllers().AddOData(options =>
        {
            options
                .Count()
                .Expand()
                .Filter()
                .OrderBy()
                .Select()
                .SetMaxTop(20)
                .AddRouteComponents("v1", edmModelV1, services =>
                {
                    // OData seems to have its own container.
                    // Register services here that are used in overridden OData implementations.
                    services.AddSingleton<ODataResourceSerializer, AuditingODataResourceSerializer>();
                    services.AddHttpContextAccessor();
                })
                .AddRouteComponents("v2", edmModelV2, services =>
                {
                    services.AddSingleton<ODataResourceSerializer, AuditingODataResourceSerializer>();
                    services.AddHttpContextAccessor();
                })
                .AddRouteComponents("v3", edmModelV3, services =>
                {
                    services.AddSingleton<ODataResourceSerializer, AuditingODataResourceSerializer>();
                    services.AddHttpContextAccessor();
                });

            // Limit routing conventions.
            var conventions = options.Conventions
                .Where(c => c is AttributeRoutingConvention || c is MetadataRoutingConvention)
                .ToList();
            
            options.Conventions.Clear();
            options.Conventions.AddRange(conventions);
        });
    }

    private static IEdmModel GetEdmModelV1()
    {
        var modelBuilder = new ODataConventionModelBuilder();

        ConfigureAuthor(modelBuilder);
        ConfigureBook(modelBuilder);

        return modelBuilder.GetEdmModel();
    }

    private static void ConfigureAuthor(ODataConventionModelBuilder modelBuilder)
    {
        var authorEntity = modelBuilder.EntityType<Author>();

        authorEntity.HasKey(author => author.Id);

        var birthday = authorEntity.Property(e => e.Birthday);
        birthday.IsRequired();
        birthday.AsDate();

        authorEntity.Property(e => e.CreatedAt);
        var firstName = authorEntity.Property(e => e.FirstName);
        firstName.IsRequired();
        firstName.MaxLength = 32;

        var lastName = authorEntity.Property(e => e.LastName);
        lastName.IsRequired();
        lastName.MaxLength = 32;

        var organizationId = authorEntity.Property(e => e.OrganizationId);
        organizationId.IsRequired();

        authorEntity.Property(e => e.ModifiedAt);
        authorEntity.Property(e => e.ModifiedBy);
        authorEntity.ContainsMany(e => e.Books);

        var authorEntitySet = modelBuilder.EntitySet<Author>("Authors");
    }

    private static void ConfigureBook(ODataConventionModelBuilder modelBuilder)
    {
        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(e => e.Id);
        bookEntity.Property(e => e.AuthorId);
        bookEntity.Property(e => e.CreatedAt);

        var datePublished = bookEntity.Property(e => e.DatePublished);
        datePublished.IsRequired();

        bookEntity.Property(e => e.ModifiedAt);
        bookEntity.Property(e => e.ModifiedBy);

        var title = bookEntity.Property(e => e.Title);
        title.IsRequired();

        bookEntity.ContainsRequired(e => e.Author);

        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");
    }

    private static IEdmModel GetEdmModelV2()
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var authorEntity = modelBuilder.EntityType<Author>();
        authorEntity.HasKey(e => e.Id);
        authorEntity.Property(e => e.Birthday);
        authorEntity.Ignore(e => e.CreatedAt);
        authorEntity.Property(e => e.FirstName);
        authorEntity.Property(e => e.LastName);
        authorEntity.Property(e => e.OrganizationId);
        authorEntity.Ignore(e => e.ModifiedAt);
        authorEntity.Ignore(e => e.ModifiedBy);
        authorEntity.ContainsMany(e => e.Books);
        var authorEntitySet = modelBuilder.EntitySet<Author>("Authors");

        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(e => e.Id);
        bookEntity.Property(e => e.AuthorId);
        bookEntity.Ignore(e => e.CreatedAt);
        bookEntity.Property(e => e.DatePublished);
        bookEntity.Ignore(e => e.ModifiedAt);
        bookEntity.Ignore(e => e.ModifiedBy);
        bookEntity.Property(e => e.Price);
        bookEntity.Property(e => e.Title);
        bookEntity.ContainsRequired(e => e.Author);
        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");

        return modelBuilder.GetEdmModel();
    }

    private static IEdmModel GetEdmModelV3()
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(e => e.Id);
        bookEntity.Property(e => e.AuthorId);
        bookEntity.Ignore(e => e.CreatedAt);
        bookEntity.Property(e => e.DatePublished);
        bookEntity.Ignore(e => e.ModifiedAt);
        bookEntity.Ignore(e => e.ModifiedBy);
        bookEntity.Property(e => e.Price);
        bookEntity.Property(e => e.Title);
        bookEntity.Ignore(e => e.Author);
        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");

        return modelBuilder.GetEdmModel();
    }
}