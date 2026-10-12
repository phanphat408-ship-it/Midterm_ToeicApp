using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ToeicReadingApp.Web.Views;

public abstract class ToeicReadingAppRazorPage<TModel> : AbpRazorPage<TModel>
{
    [RazorInject]
    public IAbpSession AbpSession { get; set; }

    protected ToeicReadingAppRazorPage()
    {
        LocalizationSourceName = ToeicReadingAppConsts.LocalizationSourceName;
    }
}
