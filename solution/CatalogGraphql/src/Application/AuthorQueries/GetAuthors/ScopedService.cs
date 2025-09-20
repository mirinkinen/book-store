namespace Application.AuthorQueries.GetAuthors;

public class ScopedService
{
    public async Task<string> GetHelloWorld()
    {
        await Task.Delay(5000);
        return "My hash code: " + GetHashCode();
    }
}