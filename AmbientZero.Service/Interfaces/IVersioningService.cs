using AmbientZero.Service.Models.Request;
using AmbientZero.Service.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    public interface IVersioningService
    {
        Task<IEnumerable<VersioningResponse>?> UpdateRemoteVersion();
        IEnumerable<VersioningResponse>? GetVersions();
        Task UpdateModule(UpdateModuleRequest request);
        Task<bool> UploadModuleFile(IFormFile file, string deviceId);
    }
}
