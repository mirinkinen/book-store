using Cataloging.Requests.Authors.Domain;
using Cataloging.Requests.Books.Domain;
using Common.API.Auditing;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Cataloging.API;

internal static class ODataConfiguration
{
    internal static void AddOData(this WebApplicationBuilder builder)
    {
        var edmModelV1 = GetEdmModelV1();
        var edmModelV2 = GetEdmModelV2();

        builder.Services.AddControllers().AddOData(
            options => options
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
                }));
    }

    private static IEdmModel GetEdmModelV1()
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var authorEntity = modelBuilder.EntityType<Author>();
        authorEntity.HasKey(author => author.Id);
        authorEntity.Property(e => e.Birthday);
        authorEntity.Property(e => e.CreatedAt);
        authorEntity.Property(e => e.FirstName);
        authorEntity.Property(e => e.LastName);
        authorEntity.Property(e => e.OrganizationId);
        authorEntity.Property(e => e.ModifiedAt);
        authorEntity.Property(e => e.ModifiedBy);
        authorEntity.ContainsMany(e => e.Books);
        var authorEntitySet = modelBuilder.EntitySet<Author>("Authors");

        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(e => e.Id);
        bookEntity.Property(e => e.AuthorId);
        bookEntity.Property(e => e.CreatedAt);
        bookEntity.Property(e => e.DatePublished);
        bookEntity.Property(e => e.ModifiedAt);
        bookEntity.Property(e => e.ModifiedBy);
        bookEntity.Property(e => e.Title);
        bookEntity.ContainsRequired(e => e.Author);
        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");

        return modelBuilder.GetEdmModel();
    }

    private static IEdmModel GetEdmModelV2()
    {
        var modelBuilder = new ODataConventionModelBuilder();

        var authorEntity = modelBuilder.EntityType<Author>();
        authorEntity.HasKey(e => e.Id);
        authorEntity.Property(e => e.Birthday);
        authorEntity.Property(e => e.FirstName);
        authorEntity.Property(e => e.LastName);
        authorEntity.Property(e => e.OrganizationId);
        authorEntity.Ignore(e => e.ModifiedBy);
        authorEntity.ContainsMany(e => e.Books);
        var authorEntitySet = modelBuilder.EntitySet<Author>("Authors");

        var bookEntity = modelBuilder.EntityType<Book>();
        bookEntity.HasKey(e => e.Id);
        bookEntity.Property(e => e.AuthorId);
        bookEntity.Property(e => e.DatePublished);
        bookEntity.Property(e => e.Title);
        bookEntity.Property(e => e.Price);
        bookEntity.Ignore(e => e.ModifiedBy);
        bookEntity.ContainsRequired(e => e.Author);
        var bookEntitySet = modelBuilder.EntitySet<Book>("Books");

        return modelBuilder.GetEdmModel();
    }
}