using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace DeepRacerProcessor.Controllers
{
    [Route("/")]
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
            return new OkObjectResult(Assembly.GetExecutingAssembly().FullName);
        }
    }
}