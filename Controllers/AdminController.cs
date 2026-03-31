using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController: ControllerBase {
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public AdminController(AppDbContext db, UserManager<ApplicationUser> userManager) {
        _db = db;
        _userManager = userManager;
    }
    private int? AdminCityId => int.TryParse(User.FindFirstValue("AdminForCityId"), out var id) ? id : null;
    [HttpPost("grant")]// POST: Grant city admin to another user
    public async Task<IActionResult> GrantAdmin([FromBody] GrantAdminDto dto) {
        if (AdminCityId == null) return Forbid();
        var targetUser = await _userManager.FindByIdAsync(dto.UserId);
        if (targetUser == null) return NotFound("User not found");
        targetUser.AdminForCityId = AdminCityId;// Can only grant admin for your own city
        var result = await _userManager.UpdateAsync(targetUser);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok($"User {targetUser.UserName} is now a city admin for city {AdminCityId}");
    }
    [HttpDelete("revoke/{userId}")]// DELETE: Revoke city admin
    public async Task<IActionResult> RevokeAdmin(string userId) {
        if (AdminCityId == null) return Forbid();
        var targetUser = await _userManager.FindByIdAsync(userId);
        if (targetUser == null) return NotFound();
        if (targetUser.AdminForCityId != AdminCityId) return Forbid();
        targetUser.AdminForCityId = null;
        await _userManager.UpdateAsync(targetUser);
        return Ok("Admin access revoked");
    }
}