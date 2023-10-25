using JWT_Auth.Data;
using JWT_Auth.Dtos;
using JWT_Auth.Helpers;
using JWT_Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTService _jWTService;

        public AuthController(IUserRepository userRepository, JWTService jWTService)
        {
            _userRepository = userRepository;
            _jWTService = jWTService;
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            return Created("success", _userRepository.Create(user));
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var user = _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid Email" });
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Password" });
            }

            var jwt = _jWTService.Generate(user.Id);

            // now front end walay only get it and send it

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new { message = "success" });
        }

        [HttpGet("User")]

        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jWTService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _userRepository.GetById(userId);

                return Ok(user);
            }
            catch (Exception)
            {

                return Unauthorized();
            }

        }

        [HttpPost("Logout")]

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "success"
            });
        }
    }
}
