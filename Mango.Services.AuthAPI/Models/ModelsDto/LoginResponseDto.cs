namespace Mango.Services.AuthAPI.Models.ModelsDto
{
	public class LoginResponseDto
	{
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
