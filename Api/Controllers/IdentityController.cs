using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Json(User.Claims.Where(x => x.Type == "phone_number").Select(x => new { x.Type, x.Value }));
        }
    }
}
