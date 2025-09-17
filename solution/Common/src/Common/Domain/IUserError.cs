using HotChocolate;

namespace Common.Domain;

[GraphQLName("UserError")]
public interface IUserError
{
    string Message { get; }

    string Code { get; }
}
