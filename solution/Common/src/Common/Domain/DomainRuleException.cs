namespace Common.Domain;

public class DomainRuleException : Exception, IUserError
{
    public string Code { get; set; }
    
    public DomainRuleException(string message) : base(message)
    {
    }
    
    public DomainRuleException(string message, string code) : base(message)
    {
        Code = code;
    }

    public DomainRuleException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DomainRuleException()
    {
    }
}