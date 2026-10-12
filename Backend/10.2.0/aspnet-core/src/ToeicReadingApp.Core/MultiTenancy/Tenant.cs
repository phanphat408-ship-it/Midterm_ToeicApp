using Abp.MultiTenancy;
using ToeicReadingApp.Authorization.Users;

namespace ToeicReadingApp.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}
