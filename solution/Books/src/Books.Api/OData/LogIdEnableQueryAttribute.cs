using Books.Domain.SeedWork;
using Microsoft.AspNetCore.OData.Query;
using System.Diagnostics;

namespace Books.Api.OData;

public sealed class LogIdEnableQueryAttribute : EnableQueryAttribute
{
    public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
    {
        var appliedQuery = base.ApplyQuery(queryable, queryOptions);
        var ids = new List<Guid>();

        if (appliedQuery is IQueryable<Entity> entityQuery)
        {
            var entities = entityQuery.Cast<Entity>().ToList();
            ids.AddRange(entities.Select(i => i.Id));
        }
        else if (appliedQuery is IQueryable<object> objectQuery)
        {
            var objects = objectQuery.Cast<object>().ToList();

            foreach (var obj in objects)
            {
                var type = obj.GetType();
                var containerProperty = type.GetProperty("Container");
                if (containerProperty == null) { break; }

                var container = containerProperty.GetValue(obj);
                if (container == null) { break; }

                Guid? id = GetIdValue(container);

                if (id == null)
                {
                    break;
                }
            }
        }

        foreach (var id in ids)
        {
            Debug.WriteLine(id);
        }

        return appliedQuery;
    }

    private static Guid? GetIdValue(object container)
    {
        var containerType = container.GetType();

        if (TryGetIdPropertyValue(container, out Guid id))
        {
            return id;
        }

        var getNextMethods = containerType.GetMethods().Where(m => m.Name.StartsWith("get_Next", StringComparison.Ordinal));

        foreach (var getNextMethod in getNextMethods)
        {
            var methodReturnValue = getNextMethod.Invoke(container, null);

            if (methodReturnValue == null)
            {
                return null;
            }

            if (TryGetIdPropertyValue(methodReturnValue, out id))
            {
                return id;
            }
        }

        return null;
    }

    private static bool TryGetIdPropertyValue(object container, out Guid id)
    {
        id = Guid.Empty;
        var containerType = container.GetType();
        var nameProperty = containerType.GetProperty("Name");
        var namePropertyValue = nameProperty?.GetValue(container) as string;

        if (namePropertyValue?.Equals("Id", StringComparison.Ordinal) == true)
        {
            var valueProperty = containerType.GetProperty("Value");

            if (valueProperty == null)
            {
                return false;
            }

            var value = valueProperty.GetValue(container);
            if (value is Guid guid)
            {
                id = guid;
                return true;
            }
        }

        return false;
    }
}