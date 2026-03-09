using System;

namespace AbpBackgroundJobDemo.DemoJob;

[Serializable]
public class DemoJobArgs
{
    public string Message { get; set; } = string.Empty;
    public DateTime EnqueuedAtUtc { get; set; }
}
