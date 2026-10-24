using System.ComponentModel;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Application.ApplicationServices.Maps_Dto;

public class RegisterDto : UserDto
{
    public string? Password { get; set; }

    [DefaultValue(Roles.Professor)]
    public string? Role { get; set; }
}