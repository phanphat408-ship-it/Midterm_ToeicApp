using ToeicReadingApp.Configuration.Dto;
using System.Threading.Tasks;

namespace ToeicReadingApp.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
