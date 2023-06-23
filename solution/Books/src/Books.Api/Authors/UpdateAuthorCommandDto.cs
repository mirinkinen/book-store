namespace Books.Api.Controllers;

public partial class AuthorsController
{
    public record UpdateAuthorCommandDto(string Firstname, string Lastname, DateTime Birthday);
}