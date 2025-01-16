namespace Common.Api.Domain;

public class DomainRuleException : Exception
{
    public DomainRuleException(string message) : base(message)
    {
    }

    public DomainRuleException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DomainRuleException()
    {
    }
}