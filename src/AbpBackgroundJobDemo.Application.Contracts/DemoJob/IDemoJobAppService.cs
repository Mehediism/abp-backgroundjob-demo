using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AbpBackgroundJobDemo.DemoJob;

public interface IDemoJobAppService : IApplicationService
{
    /// <summary>
    /// Enqueues a demo background job with the given message.
    /// </summary>
    Task EnqueueAsync(string message, TimeSpan? delay = null);
}
