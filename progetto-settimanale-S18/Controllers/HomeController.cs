using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace progetto_settimanale_S18.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
