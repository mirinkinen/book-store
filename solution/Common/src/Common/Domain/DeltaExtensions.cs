using Microsoft.AspNetCore.OData.Deltas;

namespace Common.Domain;

public static class DeltaExtensions
{
    public static bool TryGetPropertyValue<TProperty>(this Delta delta,string name, out TProperty value)
    {
        if (delta.TryGetPropertyValue(name, out var objectValue))
        {
            if (objectValue is TProperty typedValue)
            {
                value = typedValue;
                return true;
            }
        }

        value = default!;
        return false;
    }
}