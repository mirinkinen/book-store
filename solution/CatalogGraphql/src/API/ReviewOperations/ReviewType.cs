using Domain;

namespace API.ReviewOperations;

public class ReviewType : ObjectType<Review>
{
    protected override void Configure(IObjectTypeDescriptor<Review> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsImplicitly();
    }
}