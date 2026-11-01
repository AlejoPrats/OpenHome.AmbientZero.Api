using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersioningController : ControllerBase
    {
        private readonly IVersioningService _versioningService;
        public VersioningController(IVersioningService versioningService)
        {
            _versioningService = versioningService ?? throw new ArgumentNullException(nameof(versioningService));
        }

        [HttpGet]
        [Route("UpdateVersion")]
        public IActionResult UpdateVersioning()
        {
            var result = _versioningService.UpdateRemoteVersion().Result;

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetVersions")]
        public IActionResult GetVersions()
        {
            var result = _versioningService.GetVersions();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("UpdateModule")]
        public async Task<IActionResult> UpdateModule(UpdateModuleRequest request)
        {
            await _versioningService.UpdateModule(request);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("UploadModuleFile")]
        public async Task<IActionResult> UploadModuleFile(IFormFile file, [FromForm] Guid moduleId)
        {
            bool result = await _versioningService.UploadModuleFile(file, moduleId.ToString());
            if (result)
            {
                return Ok();
            }
            else
            {
                return Problem("Error uploading file");
            }
        }
    }
}
