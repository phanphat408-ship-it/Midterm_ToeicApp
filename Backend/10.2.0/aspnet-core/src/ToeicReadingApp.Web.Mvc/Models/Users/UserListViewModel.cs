using ToeicReadingApp.Roles.Dto;
using System.Collections.Generic;

namespace ToeicReadingApp.Web.Models.Users;

public class UserListViewModel
{
    public IReadOnlyList<RoleDto> Roles { get; set; }
}
