using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SingalrAPI.Services.BlobService;
using System.Text;

namespace SingalrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public HubController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet("/OnlineUsers")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            try
            {
               // Stream s = new MemoryStream(Encoding.ASCII.GetBytes("test ahmed"));
                //await _blobService.UploadBlobAsync("testhamed.txt", s);

                return Ok(await _blobService.GetAllBlobs());
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //return Ok(ApplicationHub.OnLineUsers.Select(s=>new {Id= s.Key, userName=s.Value}).ToList());
        }
    }
}
