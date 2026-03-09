# ABP Background Job Demo

## Summary

This solution demonstrates **ABP Framework Background Jobs**: a one-time, queueable background task with **logging** and an **API to enqueue** the job.

- **Background Job**: `DemoBackgroundJob` runs once per enqueue, logs execution (message and enqueued time).
- **Module that enqueues**: `AbpBackgroundJobDemo.Application` provides `DemoJobAppService` (implements `IDemoJobAppService`). It uses `IBackgroundJobManager.EnqueueAsync` to queue jobs, with optional delay.
- **Logging**: Job execution is logged in `DemoBackgroundJob.ExecuteAsync` via `ILogger<DemoBackgroundJob>`.

The project follows ABP layering: Domain.Shared, Domain, Application.Contracts, Application, HttpApi. The article in `article.md` explains what background jobs are, when to use them, and how to implement them with code examples.

## How to Run

1. **Open the solution** in Visual Studio or Rider, or from CLI:
   ```bash
   cd abp-backgroundjob-demo
   dotnet restore
   dotnet build
   ```

2. **Run inside an ABP host application.**  
   This repo is a **module** (library). To run it you need a host that references these projects and loads the modules:
   - Add project references from your host to:
     - `AbpBackgroundJobDemo.Application`
     - `AbpBackgroundJobDemo.HttpApi`
   - In your host module, add:
     - `[DependsOn(typeof(AbpBackgroundJobDemoApplicationModule))]`
     - `[DependsOn(typeof(AbpBackgroundJobDemoHttpApiModule))]`
   - Ensure the host uses the ABP Background Jobs module (default when using `Volo.Abp.BackgroundJobs`).

   If you created your app with the ABP CLI (`abp new MyApp`), add the above module dependencies and project references to the host project, then run the host (e.g. `MyApp.HttpApi.Host` or `MyApp.Web`).

3. **Alternative: run the HttpApi project in isolation**  
   The HttpApi project only references Application.Contracts. To actually execute jobs, the process must load the Application module (which registers the job and the app service). So the host must load both `AbpBackgroundJobDemoApplicationModule` and `AbpBackgroundJobDemoHttpApiModule`.

## How to Trigger the Job

- **Via HTTP API** (when the host is running):
  ```http
  POST /api/abp-background-job-demo/demo-job/enqueue
  ```
  Optional query parameters:
  - `message` – text to pass to the job (default: `"Triggered via API"`).
  - `delaySeconds` – run the job after this many seconds (optional).

  Examples:
  ```bash
  curl -X POST "https://localhost:5001/api/abp-background-job-demo/demo-job/enqueue"
  curl -X POST "https://localhost:5001/api/abp-background-job-demo/demo-job/enqueue?message=Hello&delaySeconds=5"
  ```

- **From code** (in any service that has access to `IDemoJobAppService`):
  ```csharp
  await _demoJobAppService.EnqueueAsync("My message");
  await _demoJobAppService.EnqueueAsync("Delayed message", TimeSpan.FromMinutes(1));
  ```

After the job runs, check your application logs for lines like:
`[DemoBackgroundJob] Executing. Message=... EnqueuedAtUtc=...`
