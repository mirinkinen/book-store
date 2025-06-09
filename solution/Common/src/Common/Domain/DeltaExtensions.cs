using Microsoft.AspNetCore.OData.Deltas;
using System.Linq.Expressions;

namespace Common.Domain;

public static class DeltaExtensions
{
    public static bool TryGetPropertyValue<TProperty>(this Delta delta, string name, out TProperty value)
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

    public static bool TryGetPropertyValue<TEntity, TProperty>(
        this Delta<TEntity> delta,
        Expression<Func<TEntity, TProperty>> propertySelector,
        out TProperty value) where TEntity : class
    {
        var memberExpression = propertySelector.Body switch
        {
            // In case the lambda is a simple member access (e => e.FirstName)
            MemberExpression m => m,
            // If there's a conversion (e.g., boxing), unwrap it.
            UnaryExpression { Operand: MemberExpression me } => me,
            _ => null
        };

        if (memberExpression == null)
        {
            throw new ArgumentException("The lambda expression should point to a valid property", nameof(propertySelector));
        }

        var propertyName = memberExpression.Member.Name;
        // Now delegate to your original method on Delta.

        return delta.TryGetPropertyValue(propertyName, out value);
    }
}