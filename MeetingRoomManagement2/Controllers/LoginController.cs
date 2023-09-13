using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomManagement.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
