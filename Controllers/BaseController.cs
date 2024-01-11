using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Utilities.Filter;

namespace MarketPlace.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }

    [ApiKeyAuth]
    public class ApiKeyAuthBaseController : BaseController
    {
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticationBaseController : ApiKeyAuthBaseController
    {
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminBaseController : ApiKeyAuthBaseController
    {
    }
}
