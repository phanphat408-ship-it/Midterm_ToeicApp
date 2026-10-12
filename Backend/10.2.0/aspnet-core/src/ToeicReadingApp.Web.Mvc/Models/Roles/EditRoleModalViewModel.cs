using Abp.AutoMapper;
using ToeicReadingApp.Roles.Dto;
using ToeicReadingApp.Web.Models.Common;

namespace ToeicReadingApp.Web.Models.Roles;

[AutoMapFrom(typeof(GetRoleForEditOutput))]
public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
{
    public bool HasPermission(FlatPermissionDto permission)
    {
        return GrantedPermissionNames.Contains(permission.Name);
    }
}
