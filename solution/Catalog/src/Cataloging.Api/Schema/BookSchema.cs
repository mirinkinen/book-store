using Cataloging.Api.Schema.Types;

namespace Cataloging.Api.Schema;

public class BookSchema : GraphQL.Types.Schema
{
    public BookSchema(IServiceProvider serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<BookQuery>();
    }
}