using System.Reflection;
using Cataloging.Domain.SeedWork;

namespace Cataloging.Infrastructure.Database.Setup;

internal static class EntityExtensions
{
    public static TEntity SetId<TEntity>(this TEntity entity, Guid id) where TEntity : Entity
    {
        var type = entity.GetType().BaseType;
        var idBackingFieldName = $"<{nameof(entity.Id)}>k__BackingField";
        FieldInfo? fieldInfo = type?.GetField(idBackingFieldName, BindingFlags.Instance | BindingFlags.NonPublic);

        if (fieldInfo == null)
        {
            throw new ArgumentException("ID property not found from given entity.");
        }

        fieldInfo.SetValue(entity, id);

        return entity;
    }
}