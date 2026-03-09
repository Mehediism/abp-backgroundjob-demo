using Volo.Abp.Modularity;

namespace AbpBackgroundJobDemo;

[DependsOn(typeof(AbpBackgroundJobDemoApplicationContractsModule))]
public class AbpBackgroundJobDemoHttpApiModule : AbpModule
{
}
