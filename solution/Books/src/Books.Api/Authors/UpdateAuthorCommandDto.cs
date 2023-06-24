namespace Books.Api.Controllers;

public record UpdateAuthorCommandDto(string Firstname, string Lastname, DateTime Birthday);