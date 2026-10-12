using Abp.AspNetCore.Mvc.ViewComponents;

namespace ToeicReadingApp.Web.Views;

public abstract class ToeicReadingAppViewComponent : AbpViewComponent
{
    protected ToeicReadingAppViewComponent()
    {
        LocalizationSourceName = ToeicReadingAppConsts.LocalizationSourceName;
    }
}
