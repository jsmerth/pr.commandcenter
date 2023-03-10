using Microsoft.AspNetCore.Mvc;

namespace PR.CommandCenter.Web.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class HomeController : ControllerBase
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
      return "OK";
    }
  }
}