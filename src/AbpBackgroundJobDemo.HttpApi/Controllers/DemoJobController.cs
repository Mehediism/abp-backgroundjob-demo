using System;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpBackgroundJobDemo.Controllers;

[Area(AbpBackgroundJobDemoRemoteServiceConsts.ModuleName)]
[RemoteService(Name = AbpBackgroundJobDemoRemoteServiceConsts.RemoteServiceName)]
[Route("api/abp-background-job-demo/demo-job")]
public class DemoJobController : AbpControllerBase
{
    private readonly IDemoJobAppService _demoJobAppService;

    public DemoJobController(IDemoJobAppService demoJobAppService)
    {
        _demoJobAppService = demoJobAppService;
    }

    /// <summary>
    /// Enqueues a demo background job. Optional query: delaySeconds to run after a delay.
    /// </summary>
    [HttpPost("enqueue")]
    public virtual async Task<IActionResult> EnqueueAsync([FromQuery] string? message = null, [FromQuery] int? delaySeconds = null)
    {
        TimeSpan? delay = delaySeconds.HasValue && delaySeconds.Value > 0
            ? TimeSpan.FromSeconds(delaySeconds.Value)
            : null;
        await _demoJobAppService.EnqueueAsync(message ?? "Triggered via API", delay);
        return Ok(new { enqueued = true, message = message ?? "Triggered via API", delaySeconds = delaySeconds });
    }
}
