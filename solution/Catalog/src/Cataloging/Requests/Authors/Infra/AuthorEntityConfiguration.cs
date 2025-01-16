using Cataloging.Requests.Authors.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cataloging.Requests.Authors.Infra;

public class AuthorEntityConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Authors", options => options.IsTemporal());

        builder.Property(e => e.FirstName).HasMaxLength(50);
        builder.Property(e => e.LastName).HasMaxLength(50);
    }
}