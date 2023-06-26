using Books.Domain.SeedWork;
using Microsoft.AspNetCore.OData.Query;
using System.Diagnostics;

namespace Books.Api.Controllers;

public class LogIdEnableQueryAttribute : EnableQueryAttribute
{
    public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
    {
        var appliedQuery = base.ApplyQuery(queryable, queryOptions);
        var ids = new List<Guid>();

        if (appliedQuery is IQueryable<IIdentifiable> identifiableQuery)
        {
            var identifiables = identifiableQuery.Cast<IIdentifiable>().ToList();
            ids.AddRange(identifiables.Select(i => i.Id));
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

    private Guid? GetIdValue(object container)
    {
        var containerType = container.GetType();

        if (TryGetIdPropertyValue(container, out Guid id))
        {
            return id;
        }

        var getNextMethods = containerType.GetMethods().Where(m => m.Name.StartsWith("get_Next"));

        foreach (var getNextMethod in getNextMethods)
        {
            var methodReturnValue = getNextMethod.Invoke(container, null);

            if (TryGetIdPropertyValue(methodReturnValue, out id))
            {
                return id;
            }
        }

        return null;
    }

    public bool TryGetIdPropertyValue(object container, out Guid id)
    {
        var containerType = container.GetType();
        var nameProperty = containerType.GetProperty("Name");
        var namePropertyValue = nameProperty.GetValue(container);

        if (namePropertyValue == "Id")
        {
            var valueProperty = containerType.GetProperty("Value");
            id = (Guid)valueProperty.GetValue(container);
            return true;
        }

        id = Guid.Empty;
        return false;
    }
}