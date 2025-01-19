using Cataloging.Requests.Authors.Domain;
using Common.Domain;
using Microsoft.AspNetCore.OData.Deltas;
using System.Reflection;

namespace Cataloging.Domain;

/// <summary>
/// Represents shared properties for all entities in this domain.
/// </summary>
public abstract class Entity : IIdentifiable, ITimestamped
{
    public Guid Id { get; protected set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// ModifiedBy is managed at infra layer to keep domain logic cleaner.
    /// </summary>
    public Guid ModifiedBy { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = CreatedAt;
    }

    protected Dictionary<string, object> GetUpdatedProperties(Delta<Author> delta)
    {
        ArgumentNullException.ThrowIfNull(delta);
        
        var updatablePropertyNames = GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<UpdatableAttribute>() != null)
            .Select(p => p.Name);

        var updatedPropertyNames = delta.GetChangedPropertyNames().Intersect(updatablePropertyNames);

        var updatedValues = new Dictionary<string, object>();

        foreach (var propertyName in updatedPropertyNames)
        {
            if (delta.TryGetPropertyValue(propertyName, out var value))
            {
                updatedValues.Add(propertyName, value);
            }
        }

        return updatedValues;
    }
}