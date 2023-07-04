namespace Cataloging.Application.Requests.Authors.AddAuthor;

public record AddAuthorDto(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId);