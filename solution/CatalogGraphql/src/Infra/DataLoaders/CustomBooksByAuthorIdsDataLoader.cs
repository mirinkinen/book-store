using Application.BookQueries;
using GreenDonut;

namespace Infra.DataLoaders;

public class CustomBooksByAuthorIdsDataLoader : BatchDataLoader<Guid, IEnumerable<BookNode>>
{
    private readonly IBookReadRepository _repository;

    public CustomBooksByAuthorIdsDataLoader(
        IBatchScheduler batchScheduler,
        IBookReadRepository repository,
        DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _repository = repository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, IEnumerable<BookNode>>> LoadBatchAsync(IReadOnlyList<Guid> keys,
        CancellationToken cancellationToken)
    {
        var lookup = await _repository.GetBooksByAuthorIds(keys, cancellationToken);
        return lookup.ToDictionary(g => g.Key, g => g.AsEnumerable());
    }
}