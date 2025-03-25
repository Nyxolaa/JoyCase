namespace JoyCase.Application.User.Dto
{
    public class LoginUserDto
    {
        public long UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public long RoleId { get; set; }
    }
}
