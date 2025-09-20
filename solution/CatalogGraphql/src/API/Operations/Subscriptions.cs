using Application.Types;
using Domain;

namespace API.Operations;

public class Subscriptions
{
    [Subscribe]
    [Topic(nameof(AuthorMutations.CreateAuthor))]
    public AuthorDto OnAuthorCreated([EventMessage] AuthorDto author) => author;
}