using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase {
    private readonly AppDbContext _db;
    public ReservationsController(AppDbContext db) => _db = db;
    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    private int? AdminCityId => int.TryParse(User.FindFirstValue("AdminForCityId"), out var id) ? id : null;
    [HttpGet("mine")]// GET: My reservations
    public async Task<IActionResult> GetMine() {
        var reservations = await _db.Reservations.Where(r => r.UserId == CurrentUserId).Include(r => r.Park).ThenInclude(p => p.City).OrderByDescending(r => r.Date).Select(r => new ReservationDto(r)).ToListAsync();
        return Ok(reservations);
    }
    [HttpGet("pending")]// GET: Pending reservations for admin's city parks
    public async Task<IActionResult> GetPendingForCity() {
        if (AdminCityId == null) {
            return Forbid();
        }
        var reservations = await _db.Reservations.Where(r => r.Park.CityId == AdminCityId && r.Status == ReservationStatus.Pending).Include(r => r.Park).Include(r => r.User).Select(r => new ReservationDto(r)).ToListAsync();
        return Ok(reservations);
    }
    [HttpPost]// POST: Create reservation
    public async Task<IActionResult> Create(CreateReservationDto dto) {
        var reservation = new Reservation {
            UserId = CurrentUserId,
            ParkId = dto.ParkId,
            Date = dto.Date,
            Time = dto.Time,
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Status = ReservationStatus.Pending
        };
        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMine), new ReservationDto(reservation));
    }
    [HttpPut("{id}")]// PUT: Update own reservation (only if pending)
    public async Task<IActionResult> Update(int id, CreateReservationDto dto) {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation == null) {
            return NotFound();
        }
        if (reservation.UserId != CurrentUserId) {
            return Forbid();
        }
        if (reservation.Status != ReservationStatus.Pending) {
            return BadRequest("Only pending reservations can be updated.");
        }
        reservation.Date = dto.Date;
        reservation.Time = dto.Time;
        reservation.Name = dto.Name;
        reservation.Email = dto.Email;
        reservation.PhoneNumber = dto.PhoneNumber;
        await _db.SaveChangesAsync();
        return Ok(new ReservationDto(reservation));
    }
    [HttpDelete("{id}")]// DELETE: Delete own reservation
    public async Task<IActionResult> Delete(int id) {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation == null) {
            return NotFound();
        }
        if (reservation.UserId != CurrentUserId) {
            return Forbid();
        }
        _db.Reservations.Remove(reservation);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpPatch("{id}/status")]// PATCH: Approve or deny — admin only, cannot approve own request
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] ReservationStatus newStatus) {
        if (AdminCityId == null) {
            return Forbid();
        }
        var reservation = await _db.Reservations.Include(r => r.Park).FirstOrDefaultAsync(r => r.Id == id);
        if (reservation == null) {
            return NotFound();
        }
        if (reservation.Park.CityId != AdminCityId) {
            return Forbid();
        }
        if (reservation.UserId == CurrentUserId) {// Key rule: admin cannot approve/deny their own reservation
            return BadRequest("City admins cannot approve or deny their own reservations.");
        }
        reservation.Status = newStatus;
        await _db.SaveChangesAsync();
        return Ok(new ReservationDto(reservation));
    }
}