using InstagramClone.API.DTOs;
using InstagramClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InstagramClone.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] UserDTO userRegisterDto)
        {
            var user = _userService.Register(userRegisterDto.Username, userRegisterDto.Password, userRegisterDto.NickName);

            if (user == null)
                return BadRequest("Usuario ya existe o datos inválidos.");

            return Ok(user);
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] UserDTO userLoginDto)
        {
            var token = _userService.Authenticate(userLoginDto.Username, userLoginDto.Password);

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Credenciales inválidas.");

            return Ok(new { Token = token });
        }
    }
}
