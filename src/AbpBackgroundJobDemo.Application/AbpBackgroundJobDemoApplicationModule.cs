using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace AbpBackgroundJobDemo;

[DependsOn(
    typeof(AbpBackgroundJobDemoApplicationContractsModule),
    typeof(AbpBackgroundJobDemoDomainModule),
    typeof(Volo.Abp.BackgroundJobs.AbpBackgroundJobsModule)
)]
public class AbpBackgroundJobDemoApplicationModule : AbpModule
{
}
