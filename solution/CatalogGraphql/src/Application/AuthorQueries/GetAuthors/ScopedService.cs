namespace Application.AuthorQueries.GetAuthors;

public class ScopedService
{
    public async Task<string> GetValue()
    {
        await Task.Delay(5000);
        return "HashCode: " + GetHashCode();
    }
}