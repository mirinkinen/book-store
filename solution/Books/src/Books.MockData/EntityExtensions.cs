using Books.Domain.SeedWork;
using System.Reflection;

namespace Books.MockData;

internal static class EntityExtensions
{
    public static void SetId(this Entity entity, Guid id)
    {
        var type = entity.GetType().BaseType;
        var idBackingFieldName = $"<{nameof(entity.Id)}>k__BackingField";
        FieldInfo? fieldInfo = type?.GetField(idBackingFieldName, BindingFlags.Instance | BindingFlags.NonPublic);

        if (fieldInfo == null)
        {
            throw new ArgumentException("ID property not found from given entity.");
        }

        fieldInfo.SetValue(entity, id);
    }
}