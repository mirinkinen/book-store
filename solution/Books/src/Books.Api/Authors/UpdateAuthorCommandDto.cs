namespace Books.Api.Authors;

public record UpdateAuthorCommandDto(string Firstname, string Lastname, DateTime Birthday);