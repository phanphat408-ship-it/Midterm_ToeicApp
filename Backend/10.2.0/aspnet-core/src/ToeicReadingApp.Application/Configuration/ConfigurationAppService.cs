using Abp.Authorization;
using Abp.Runtime.Session;
using ToeicReadingApp.Configuration.Dto;
using System.Threading.Tasks;

namespace ToeicReadingApp.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : ToeicReadingAppAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
