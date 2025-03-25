using JoyCase.Data;

namespace JoyCase.Application.User.Dto
{
    public class LoginUserDto
    {
        public long UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public long RoleId { get; set; }
        public List<UserPermissionDto> UserPermissions { get; set; }
        public List<RolePermissionDto> RolePermissions { get; set; }
    }
}
