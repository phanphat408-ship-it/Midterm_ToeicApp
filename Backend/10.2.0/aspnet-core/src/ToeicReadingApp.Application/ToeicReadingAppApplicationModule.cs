using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ToeicReadingApp.Authorization;

namespace ToeicReadingApp;

[DependsOn(
    typeof(ToeicReadingAppCoreModule),
    typeof(AbpAutoMapperModule))]
public class ToeicReadingAppApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<ToeicReadingAppAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(ToeicReadingAppApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
