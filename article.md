# ABP Framework: Background Jobs

## What Are Background Jobs in ABP?

**Background Jobs** in the ABP Framework are **one-time** asynchronous tasks that can be **queued**, **delayed**, or **scheduled** for a specific time. They run in the background so that the main application thread is not blocked.

Key characteristics:

- **One-time execution** — Each job runs once and completes (unlike Background Workers, which run on a fixed schedule repeatedly).
- **Queueable** — You can enqueue many instances of the same job (e.g. one email per user).
- **Scheduling** — Jobs can be run immediately, after a delay, or at a specific time.
- **Infrastructure** — ABP provides an in-memory implementation by default; you can also use **Quartz**, **RabbitMQ**, or **Hangfire** for persistence and distributed execution.

Official documentation: [Background Jobs](https://abp.io/docs/latest/framework/infrastructure/background-jobs).  
Related: [Background Jobs vs Background Workers](https://abp.io/community/articles/abp-framework-background-jobs-vs-background-workers-when-to-use-which-t98pzjv6) (ABP Community).

---

## When to Use Background Jobs

Use **Background Jobs** when:

- You need to **queue multiple instances** of the same task (e.g. send welcome email to each new user).
- You want **built-in retry** behavior (failed jobs can be retried).
- Execution should be **guaranteed** and, with the right provider, **survive application restarts**.
- The work is **one-time** and should not block the request (e.g. process upload, generate report, send notification).

Use **Background Workers** instead when:

- The task must run **periodically** on a fixed schedule (e.g. every 5 minutes).
- It runs for the **lifetime of the application** (health checks, cleanup, polling).

---

## How to Implement a Background Job

### 1. Define the job arguments (optional but recommended)

Create a serializable class to pass data into the job:

```csharp
[Serializable]
public class DemoJobArgs
{
    public string Message { get; set; }
    public Guid? UserId { get; set; }
}
```

### 2. Implement the job

Implement `AsyncBackgroundJob<TArgs>` and use `ExecuteAsync`:

```csharp
public class DemoBackgroundJob : AsyncBackgroundJob<DemoJobArgs>
{
    private readonly ILogger<DemoBackgroundJob> _logger;

    public DemoBackgroundJob(ILogger<DemoBackgroundJob> logger)
    {
        _logger = logger;
    }

    public override async Task ExecuteAsync(DemoJobArgs args)
    {
        _logger.LogInformation("Demo job executing: {Message}", args.Message);
        await Task.CompletedTask;
    }
}
```

### 3. Enqueue the job

Inject `IBackgroundJobManager` and enqueue with optional delay:

```csharp
// Run as soon as possible
await _backgroundJobManager.EnqueueAsync(new DemoJobArgs
{
    Message = "Hello from background",
    UserId = currentUserId
});

// Run after a delay
await _backgroundJobManager.EnqueueAsync(
    new DemoJobArgs { Message = "Delayed hello" },
    delay: TimeSpan.FromMinutes(5)
);
```

---

## Code Examples

### Simple job (in-memory queue)

```csharp
[Serializable]
public class SimpleEmailJobArgs
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class SimpleEmailJob : AsyncBackgroundJob<SimpleEmailJobArgs>
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<SimpleEmailJob> _logger;

    public SimpleEmailJob(IEmailSender emailSender, ILogger<SimpleEmailJob> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public override async Task ExecuteAsync(SimpleEmailJobArgs args)
    {
        _logger.LogInformation("Sending email to {To}", args.To);
        await _emailSender.SendAsync(args.To, args.Subject, args.Body);
    }
}
```

### Queue execution (enqueue multiple jobs)

```csharp
// In an application service or controller
public async Task OnboardUserAsync(Guid userId)
{
    var user = await _userRepository.GetAsync(userId);
    // Enqueue one job per user — each runs once in the background
    await _backgroundJobManager.EnqueueAsync(new SimpleEmailJobArgs
    {
        To = user.Email,
        Subject = "Welcome",
        Body = "Thanks for signing up."
    });
    await _backgroundJobManager.EnqueueAsync(new SyncExternalDataJobArgs
    {
        UserId = userId
    }, delay: TimeSpan.FromMinutes(1));
}
```

---

## Summary

| Aspect            | Background Job                    | Background Worker              |
|------------------|------------------------------------|--------------------------------|
| Execution        | One-time                           | Repeating (e.g. every X minutes) |
| Trigger          | Enqueue when needed                | Timer / schedule                |
| Persistence      | Yes (with Quartz/RabbitMQ/Hangfire)| Typically in-memory            |
| Use cases        | Emails, file processing, reports   | Health checks, cleanup, polling |

Use **Background Jobs** for fire-and-forget, retriable, one-time work; use **Background Workers** for recurring tasks that run as long as the app runs.
