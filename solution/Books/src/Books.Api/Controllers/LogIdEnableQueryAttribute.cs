using Books.Domain.SeedWork;
using Microsoft.AspNetCore.OData.Query;
using System.Diagnostics;
using System.Reflection;

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

                if(id == null)
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
        while (true)
        {
            var containerType = container.GetType();
            PropertyInfo nameProperty = containerType.GetProperty("Name");
            var name = nameProperty.GetValue(container);

            if (name != "Id")
            {
                var nextMethod = containerType.GetMethod("get_Next0");

                if (nextMethod == null)
                {
                    return null;
                }

                container = nextMethod.Invoke(container, null);
                continue;
            }

            var valueProperty = containerType.GetProperty("Value");

            return (Guid)valueProperty.GetValue(container);
        }
    }
}