using AmbientZero.Api.ScheduledTasks;

namespace AmbientZero.Api.Extensions
{
    public static class ScheduleJobsExtensions
    {
        public static void AddScheduledJobs(this IServiceCollection services)
        {
            services.AddTransient<RebootJob>();
        }
    }
}
