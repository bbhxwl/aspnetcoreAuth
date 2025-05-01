using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcoreAuth;

[Authorize(Roles = "user")]
[Route("v1/[controller]_[action]")]      // ← 这里声明了属性路由前缀
[ApiExplorerSettings(GroupName = "v1")]
public class BaseApi: Controller
{
    
}