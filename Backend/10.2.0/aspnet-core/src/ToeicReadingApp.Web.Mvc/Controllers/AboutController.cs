using Abp.AspNetCore.Mvc.Authorization;
using ToeicReadingApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ToeicReadingApp.Web.Controllers;

[AbpMvcAuthorize]
public class AboutController : ToeicReadingAppControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
