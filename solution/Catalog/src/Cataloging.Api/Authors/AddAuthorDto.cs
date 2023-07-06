namespace Cataloging.Api.Authors;

public record AddAuthorDto(string Firstname, string Lastname, DateTime Birthday, Guid OrganizationId);