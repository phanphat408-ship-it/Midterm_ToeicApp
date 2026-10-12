using Abp.AutoMapper;
using ToeicReadingApp.Sessions.Dto;

namespace ToeicReadingApp.Web.Views.Shared.Components.TenantChange;

[AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
public class TenantChangeViewModel
{
    public TenantLoginInfoDto Tenant { get; set; }
}
