using ToeicReadingApp.Roles.Dto;
using System.Collections.Generic;

namespace ToeicReadingApp.Web.Models.Roles;

public class RoleListViewModel
{
    public IReadOnlyList<PermissionDto> Permissions { get; set; }
}
