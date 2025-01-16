namespace Cataloging.Requests.Authors.Application.AddAuthor;

public record AddAuthorDto(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId);