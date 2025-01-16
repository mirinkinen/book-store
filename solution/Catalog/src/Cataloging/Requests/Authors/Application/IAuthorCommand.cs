namespace Cataloging.Requests.Authors.Application;

public interface IAuthorCommand
{
    Guid AuthorId { get; }
}