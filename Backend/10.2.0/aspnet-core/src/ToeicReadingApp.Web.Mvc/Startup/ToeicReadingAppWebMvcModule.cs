using Abp.Modules;
using Abp.Reflection.Extensions;
using ToeicReadingApp.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ToeicReadingApp.Web.Startup;

[DependsOn(typeof(ToeicReadingAppWebCoreModule))]
public class ToeicReadingAppWebMvcModule : AbpModule
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfigurationRoot _appConfiguration;

    public ToeicReadingAppWebMvcModule(IWebHostEnvironment env)
    {
        _env = env;
        _appConfiguration = env.GetAppConfiguration();
    }

    public override void PreInitialize()
    {
        Configuration.Navigation.Providers.Add<ToeicReadingAppNavigationProvider>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ToeicReadingAppWebMvcModule).GetAssembly());
    }
}
