using Cataloging.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cataloging.Infra;

public class BookEntityConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .ToTable("Book", "Catalog", options => options.IsTemporal())
            .HasOne(book => book.Author)
            .WithMany(author => author.Books)
            .HasForeignKey(book => book.AuthorId);

        builder.Property(e => e.Title).HasMaxLength(256);
        builder.Property(e => e.Price).HasPrecision(18, 2);
    }
}