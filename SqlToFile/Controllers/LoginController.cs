using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SqlToFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public LoginController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {
            var existingUser = await _userService.GetByUsernameAsync(user.Username);
            if (existingUser == null || existingUser.Password != user.Password)
                return Unauthorized(new { message = "Kullanıcı adı veya şifre yanlış." });

            var token = GenerateJwtToken(existingUser);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel user)
        {
            var existingUser = await _userService.GetByUsernameAsync(user.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Bu kullanıcı adı zaten kayıtlı." });

            var newUser = new User { Username = user.Username, Password = user.Password };
            await _userService.AddAsync(newUser);
            return Ok(new { message = "Kayıt başarılı!" });
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username, [FromBody] UserUpdateDto updatedDto)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            updatedDto.Username = username;
            var result = await _userService.UpdateAsync(updatedDto);

            return Ok(new { message = "Kullanıcı güncellendi." });
        }
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null)
                return NotFound(new { message = "Kullanıcı bulunamadı." });

            await _userService.DeleteAsync(user.Id);
            return Ok(new { message = "Kullanıcı silindi." });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
