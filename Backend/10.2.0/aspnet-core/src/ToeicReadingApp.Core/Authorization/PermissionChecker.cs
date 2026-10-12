using Abp.Authorization;
using ToeicReadingApp.Authorization.Roles;
using ToeicReadingApp.Authorization.Users;

namespace ToeicReadingApp.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
