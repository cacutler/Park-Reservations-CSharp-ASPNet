using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[ApiController]
[Route("api/[controller]")]
public class ParksController : ControllerBase {
    private readonly AppDbContext _db;
    public ParksController(AppDbContext db) => _db = db;
    private int? AdminCityId => int.TryParse(User.FindFirstValue("AdminForCityId"), out var id) ? id : null;
    [HttpGet]// GET: All parks (public)
    public async Task<IActionResult> GetAll([FromQuery] int? cityId) {
        var query = _db.Parks.Include(p => p.City).AsQueryable();
        if (cityId.HasValue) query = query.Where(p => p.CityId == cityId.Value);
        return Ok(await query.Select(p => new ParkDto(p)).ToListAsync());
    }
    [HttpGet("{id}")]// GET: Single park with schedule
    public async Task<IActionResult> Get(int id) {
        var park = await _db.Parks.Include(p => p.City).FirstOrDefaultAsync(p => p.Id == id);
        return park == null ? NotFound() : Ok(new ParkDto(park));
    }
    [HttpPost]// POST: Create park — admin only
    [Authorize]
    public async Task<IActionResult> Create(CreateParkDto dto) {
        if (AdminCityId == null) {
            return Forbid();
        }
        var park = new Park {
            CityId = AdminCityId.Value,
            Name = dto.Name,
            Address = dto.Address,
            Schedule = dto.Schedule
        };
        _db.Parks.Add(park);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = park.Id }, new ParkDto(park));
    }
    [HttpPut("{id}")]// PUT: Update park — admin only for their city
    [Authorize]
    public async Task<IActionResult> Update(int id, CreateParkDto dto) {
        var park = await _db.Parks.FindAsync(id);
        if (park == null) return NotFound();
        if (AdminCityId == null || AdminCityId != park.CityId) {
            return Forbid();
        }
        park.Name = dto.Name;
        park.Address = dto.Address;
        park.Schedule = dto.Schedule;
        await _db.SaveChangesAsync();
        return Ok(new ParkDto(park));
    }
}