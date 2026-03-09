using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace AbpBackgroundJobDemo.DemoJob;

/// <summary>
/// Background job that logs execution. Used to demonstrate ABP Background Jobs.
/// </summary>
public class DemoBackgroundJob : AsyncBackgroundJob<DemoJobArgs>, ITransientDependency
{
    private readonly ILogger<DemoBackgroundJob> _logger;

    public DemoBackgroundJob(ILogger<DemoBackgroundJob> logger)
    {
        _logger = logger;
    }

    public override async Task ExecuteAsync(DemoJobArgs args)
    {
        _logger.LogInformation(
            "[DemoBackgroundJob] Executing. Message={Message}, EnqueuedAtUtc={EnqueuedAtUtc}",
            args.Message,
            args.EnqueuedAtUtc);

        await Task.CompletedTask;
    }
}
