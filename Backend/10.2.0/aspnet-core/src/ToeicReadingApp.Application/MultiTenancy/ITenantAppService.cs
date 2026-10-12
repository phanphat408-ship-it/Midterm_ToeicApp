using Abp.Application.Services;
using ToeicReadingApp.MultiTenancy.Dto;

namespace ToeicReadingApp.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

