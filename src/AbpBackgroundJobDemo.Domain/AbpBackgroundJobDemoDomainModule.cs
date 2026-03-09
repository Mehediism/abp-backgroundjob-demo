using Volo.Abp.Modularity;

namespace AbpBackgroundJobDemo;

[DependsOn(typeof(AbpBackgroundJobDemoDomainSharedModule))]
public class AbpBackgroundJobDemoDomainModule : AbpModule
{
}
