using HelloEmpty.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloEmpty.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HelloMessage msg = new HelloMessage()
            {
                Message = "Welcome to ASP.NET Core!"
            };

            ViewBag.Noti = "Input message and click submit";

            return View(msg);
        }
    }
}
