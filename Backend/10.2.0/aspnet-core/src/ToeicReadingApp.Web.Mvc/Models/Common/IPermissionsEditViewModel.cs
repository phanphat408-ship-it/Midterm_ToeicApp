using ToeicReadingApp.Roles.Dto;
using System.Collections.Generic;

namespace ToeicReadingApp.Web.Models.Common;

public interface IPermissionsEditViewModel
{
    List<FlatPermissionDto> Permissions { get; set; }
}