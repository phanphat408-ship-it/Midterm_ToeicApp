using Abp.Modules;
using Abp.Reflection.Extensions;
using ToeicReadingApp.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ToeicReadingApp.Web.Host.Startup
{
    [DependsOn(
       typeof(ToeicReadingAppWebCoreModule))]
    public class ToeicReadingAppWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ToeicReadingAppWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ToeicReadingAppWebHostModule).GetAssembly());
        }
    }
}
