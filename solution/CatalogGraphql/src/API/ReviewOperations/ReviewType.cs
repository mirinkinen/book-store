using Domain;

namespace API.ReviewOperations;

public class ReviewType : ObjectType<Review>
{
    protected override void Configure(IObjectTypeDescriptor<Review> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        descriptor.Field(b => b.Id);
        descriptor.Field(b => b.Title);
        descriptor.Field(b => b.Body);
        descriptor.Field(b => b.BookId);
        descriptor.Field(b => b.Book);
    }
}