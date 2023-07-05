using Common.Application.Authentication;

namespace Cataloging.Application.Requests.Authors.DeleteAuthor;

public record AuthorDeleted(Guid AuthorId);