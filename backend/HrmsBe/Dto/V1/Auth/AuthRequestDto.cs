namespace HrmsBe.Dto.V1.Auth
{
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class ChangePasswordDto
    {
        public string UserId { get; set; } = "";
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }

    public class UpdateProfileDto
    {
        public string UserId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }

    public class UserProfileDto
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }
    public class MenuDto
    {
        public int FirstLevelMenuId { get; set; }
        public string Name { get; set; } = "";
        public string Link { get; set; } = "";
        public List<SecondLevelMenuDto> SecondLevelMenu { get; set; } = [];
    }

    public class SecondLevelMenuDto
    {
        public int SecondLevelMenuId { get; set; }
        public string Name { get; set; } = "";
        public string Link { get; set; } = "";
    }


}
