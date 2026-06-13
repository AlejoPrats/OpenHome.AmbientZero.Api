using AmbientZero.Api.Extensions;
using AmbientZero.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BackupController : ControllerBase
    {
        private readonly IBackupService _backupService;

        public BackupController(IBackupService backupService)
        {
            _backupService = backupService ?? throw new ArgumentNullException(nameof(backupService));
        }

        [HttpPost]
        [Route("CreateBackup")]
        public IActionResult CreateBackup(string[] deviceIds)
        {
            var result = _backupService.GenerateBackupFile(deviceIds);

            if (result is null)
            {
                return NotFound();
            }

            HttpContext.SetFileDownloadHeader(result.Filename);
            return new FileStreamResult(result.BackupStream, "application/octet-stream");
        }

        [HttpPost]
        public IActionResult ImportBackup(IFormFile backup)
        {
            var result = _backupService.ImportBackup(backup.FileName, backup.OpenReadStream());

            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
