using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ToeicReadingApp.Controllers
{
    public abstract class ToeicReadingAppControllerBase : AbpController
    {
        protected ToeicReadingAppControllerBase()
        {
            LocalizationSourceName = ToeicReadingAppConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
