using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config) {
        _userManager = userManager;
        _config = config;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto) {
        var user = new ApplicationUser {
            Name = dto.Name,
            Email = dto.Email,
            UserName = dto.Username,
            PhoneNumber = dto.PhoneNumber
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok("Registered successfully");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto) {
        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password)){
            return Unauthorized("Invalid credentials");
        }
        var token = GenerateJwt(user);
        return Ok(new {token, userId = user.Id, isAdmin = user.AdminForCityId != null});
    }
    private string GenerateJwt(ApplicationUser user) {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim("AdminForCityId", user.AdminForCityId?.ToString() ?? "")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"], claims: claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}