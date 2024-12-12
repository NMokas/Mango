using Mango.Services.AuthAPI.Models.ModelsDto;

namespace Mango.Services.AuthAPI.Service.IService
{
	public interface IAuthService
	{
		Task<string> Resgiter(RegistrationRequestDto registrationRequestDto);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

		Task<bool> AssignRole(string email,string roleName);

	}
}
