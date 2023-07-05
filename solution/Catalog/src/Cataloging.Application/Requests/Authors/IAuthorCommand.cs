namespace Cataloging.Application.Requests.Authors;

public interface IAuthorCommand
{
    Guid AuthorId { get; }
}