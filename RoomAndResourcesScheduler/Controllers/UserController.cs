using Microsoft.AspNetCore.Mvc;

namespace RoomAndResourcesScheduler.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
