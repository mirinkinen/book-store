using System.Diagnostics.CodeAnalysis;

namespace Cataloging.IntegrationTests;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This is used in tests.")]
internal class ErrorResponse
{
    public Error Error { get; set; }
}

[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Used by OData")]
public class Error
{
    public string Code { get; set; }

    public string Message { get; set; }

    public IList<object> Details { get; }

    public InnerError InnerError { get; set; }
}

public class InnerError
{
    public string Message { get; set; }

    public string Type { get; set; }

    public string Stacktrace { get; set; }
}