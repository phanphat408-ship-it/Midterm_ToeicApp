using Abp.AspNetCore.Mvc.Authorization;
using ToeicReadingApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ToeicReadingApp.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : ToeicReadingAppControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
