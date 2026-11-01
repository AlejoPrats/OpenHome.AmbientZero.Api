using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Services;
using Coravel.Invocable;
using System.Diagnostics;

namespace AmbientZero.Api.ScheduledTasks
{
    public class RebootJob : IInvocable
    {
        private readonly ISystemService _systemService;

        public RebootJob(ISystemService systemService)
        {
            _systemService = systemService ?? throw new ArgumentNullException(nameof(systemService));
        }

        public async Task Invoke()
        {
            await _systemService.ScheduleReboot();
        }
    }
}
