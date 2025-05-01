using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcoreAuth;

public class Api: BaseApi
{

    [HttpPost]
    [Authorize(Roles = "admin")]
    public ActionResult Test1(string u)
    {
        return Ok();
    }
}