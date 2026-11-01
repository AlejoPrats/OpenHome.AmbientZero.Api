using AmbientZero.Api.Extensions;
using AmbientZero.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

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

        [HttpGet]
        [Route("GenerateBackup")]
        public IActionResult GenerateBackup()
        {
            _backupService.GenerateBackup();
            return Ok();
        }

        [HttpPost]
        public IActionResult UploadBackupFile(IFormFile file)
        {
            var result = _backupService.BackupFileUpload(file.OpenReadStream());

            if (result == false)
            {
                return Problem();
            }

            return Ok();
        }

        [HttpGet]
        [Route("DownloadBackup")]
        public IActionResult DownloadBackup()
        {
            var result = _backupService.GetFileStream();

            if (result is null)
            {
                return NotFound();
            }

            return File(result.BackupStream, "application/octet-stream", result.Filename);
        }

        [HttpPost]
        [Route("ImportBackup")]
        public IActionResult ImportBackup(IFormFile backup)
        {
            _backupService.ImportBackupFile();
            return Ok();
        }
    }
}
