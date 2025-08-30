using Microsoft.AspNetCore.OData.Deltas;
using System.Linq.Expressions;

namespace Cataloging.Domain;

public static class DeltaExtensions
{
    /// <summary>
    /// Returns true if the property with the specified name has been changed in the Delta object. The actual value of the property is
    /// returned in the out parameter.
    /// </summary>
    public static bool TryGetChangedPropertyValue<TProperty>(this Delta delta, string propertyName, out TProperty value)
    {
        // Non-nullable types like DateTime will have a default value in Delta object, if the value is not provided as PATCH payload.
        // This causes TryGetPropertyValue method to return true, even the value was not provided by client.
        // This check ensures that the property is actually changed in the Delta object.
        if (!delta.GetChangedPropertyNames().Contains(propertyName))
        {
            value = default!;
            return false;
        }

        if (delta.TryGetPropertyValue(propertyName, out var objectValue))
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

    /// <summary>
    /// Returnr true if the property with the specified selector has been changed in the Delta object. he actual value of the property is
    /// returned in the out parameter.
    /// </summary>
    public static bool TryGetChangedPropertyValue<TEntity, TProperty>(
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

        return delta.TryGetChangedPropertyValue(propertyName, out value);
    }
}