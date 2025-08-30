using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Cataloging.IntegrationTests;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This is used in tests.")]
internal class ValueResponse<TValue>
{
    [JsonPropertyName("@odata.context")]
    public string Context { get; set; }

    [JsonPropertyName("value")]
    public List<TValue> Value { get; set; }
}