using Microsoft.AspNetCore.Mvc;

namespace OprawaObrazow.Modules.Health;

[ApiController]
[Route( "api-main/[controller]" )]
public class HealthController : ControllerBase
{
  [HttpGet]
  public ActionResult<string> Get() => "OK";
}