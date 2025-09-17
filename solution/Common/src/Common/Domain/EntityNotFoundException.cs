namespace Common.Domain;

public class EntityNotFoundException : Exception, IUserError
{
    public string Code { get; set; }
    
    public EntityNotFoundException(string message) : base(message)
    {
    }
    
    public EntityNotFoundException(string message, string code) : base(message)
    {
        Code = code;
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public EntityNotFoundException()
    {
    }
}