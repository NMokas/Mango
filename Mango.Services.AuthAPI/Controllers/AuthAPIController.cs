using Mango.Services.AuthAPI.Models.ModelsDto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthAPIController : ControllerBase
	{
		private readonly IAuthService _authService;
		protected ResponseDto _responseDto;

        public AuthAPIController(IAuthService authService)
        {
            _authService=authService;
			_responseDto = new ResponseDto();
        }
        [HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
		{
			var errorMessage = await _authService.Resgiter(model);
			if(!string.IsNullOrEmpty(errorMessage)) 
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message=errorMessage;
				return BadRequest(_responseDto);	
			}
			return Ok(_responseDto);
		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
		{

			var loginResponse= await _authService.Login(model);

			if(loginResponse.User==null&& loginResponse.Token == "")
			{
				_responseDto.IsSuccess=false;
				_responseDto.Message = "Login Invalid";
				return BadRequest(_responseDto);
			}
			_responseDto.Result=loginResponse;
			_responseDto.IsSuccess = true;
			_responseDto.Message = "Login Valid";

			return Ok(_responseDto);

		}
		[HttpPost("AssignRole")]
		public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
		{

			var assignRoleSuccessful = await _authService.AssignRole(model.Email,model.Role.ToUpper());

			if (!assignRoleSuccessful)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Error encountered";
				return BadRequest(_responseDto);
			}
			return Ok(_responseDto);

		}

	}
}
