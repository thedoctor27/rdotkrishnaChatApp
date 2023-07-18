using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SingalrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubController : ControllerBase
    {

        public HubController()
        {
               
        }

        [HttpGet("/OnlineUsers")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            return Ok(ApplicationHub.OnLineUsers.Select(s=>new {Id= s.Key, userName=s.Value}).ToList());
        }
    }
}
