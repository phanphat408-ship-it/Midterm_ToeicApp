using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ToeicReadingApp.EntityFrameworkCore;
using ToeicReadingApp.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ToeicReadingApp.Web.Tests;

[DependsOn(
    typeof(ToeicReadingAppWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class ToeicReadingAppWebTestModule : AbpModule
{
    public ToeicReadingAppWebTestModule(ToeicReadingAppEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ToeicReadingAppWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(ToeicReadingAppWebMvcModule).Assembly);
    }
}