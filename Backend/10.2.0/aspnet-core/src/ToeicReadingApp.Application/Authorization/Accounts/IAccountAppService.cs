using Abp.Application.Services;
using ToeicReadingApp.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace ToeicReadingApp.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
