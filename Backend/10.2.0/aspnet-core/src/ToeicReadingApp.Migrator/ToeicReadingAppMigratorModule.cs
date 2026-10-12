using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ToeicReadingApp.Configuration;
using ToeicReadingApp.EntityFrameworkCore;
using ToeicReadingApp.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace ToeicReadingApp.Migrator;

[DependsOn(typeof(ToeicReadingAppEntityFrameworkModule))]
public class ToeicReadingAppMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public ToeicReadingAppMigratorModule(ToeicReadingAppEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(ToeicReadingAppMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            ToeicReadingAppConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ToeicReadingAppMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
