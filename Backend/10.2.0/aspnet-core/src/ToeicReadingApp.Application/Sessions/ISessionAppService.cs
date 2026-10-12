using Abp.Application.Services;
using ToeicReadingApp.Sessions.Dto;
using System.Threading.Tasks;

namespace ToeicReadingApp.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
