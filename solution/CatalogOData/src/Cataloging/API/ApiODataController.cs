using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Cataloging.API;

/// <summary>
/// Base class for OData controllers using API conventions. Methods in the controller must use attribute routing.
/// </summary>
[ApiController]
public class ApiODataController : ODataController
{
}