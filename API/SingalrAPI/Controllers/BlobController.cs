using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SingalrAPI.Services.BlobService;
using SingalrAPI.Services.UserService;

namespace SingalrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;
        private readonly IHubContext<ApplicationHub> _hubContext;
        public BlobController(IBlobService blobService, IHubContext<ApplicationHub> hubContext)
        {
            _blobService = blobService;
            _hubContext = hubContext;
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> UploadBlob(string userName,IFormFile file)
        {
            AppUser user = AppUser.appUsers.Where(a=>a.UserName == userName).FirstOrDefault();
            if(user == null)
            {
                return NotFound();
            }
            using var stream = file.OpenReadStream();
            await _blobService.UploadBlobAsync(user.Id+"_"+file.FileName, stream);

            await _hubContext.Clients.All.SendAsync("fileUploaded", userName + " has uploaded a file named : " + file.FileName);

            return Ok("Blob uploaded successfully!");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlobs()
        {
            return Ok(await _blobService.GetAllBlobs());
        }
        [HttpGet("{blobName}")]
        public async Task<IActionResult> DownloadBlob(string blobName)
        {
            string userName  = blobName.Split('_')[0];
            string userId = AppUser.appUsers.Where(u => u.UserName == userName).FirstOrDefault()?.Id;

            if (!string.IsNullOrEmpty(userId))
            {
                blobName = blobName.Replace(userName, userId);
            }

            var stream = await _blobService.DownloadBlobAsync(blobName);
            return File(stream, "application/octet-stream", blobName);
        }

        [HttpDelete("{userName}/{blobName}")]
        public async Task<IActionResult> DeleteBlob(string userName, string blobName)
        {
            AppUser user = AppUser.appUsers.Where(a => a.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            await _blobService.DeleteBlobAsync(user.Id + "_" + blobName);
            return Ok("Blob deleted successfully!");
        }
    }
}
