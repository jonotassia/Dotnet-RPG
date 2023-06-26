using Dotnet_RPG.Models;

namespace Dotnet_RPG.Dtos.Auth
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
