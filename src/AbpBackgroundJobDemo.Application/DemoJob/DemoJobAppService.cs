using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpBackgroundJobDemo.DemoJob;

public class DemoJobAppService : ApplicationService, IDemoJobAppService
{
    private readonly IBackgroundJobManager _backgroundJobManager;

    public DemoJobAppService(IBackgroundJobManager backgroundJobManager)
    {
        _backgroundJobManager = backgroundJobManager;
    }

    public virtual async Task EnqueueAsync(string message, TimeSpan? delay = null)
    {
        var args = new DemoJobArgs
        {
            Message = message ?? "Hello from background job",
            EnqueuedAtUtc = DateTime.UtcNow
        };

        if (delay.HasValue && delay.Value > TimeSpan.Zero)
        {
            await _backgroundJobManager.EnqueueAsync(args, delay: delay.Value);
        }
        else
        {
            await _backgroundJobManager.EnqueueAsync(args);
        }
    }
}
