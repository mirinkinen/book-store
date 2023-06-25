using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Books.Api.Tests;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This is used in tests.")]
internal class ValueResponse<TValue>
{
    [JsonProperty("odata.metadata")]
    public string Metadata { get; set; }

    public List<TValue> Value { get; set; }
}