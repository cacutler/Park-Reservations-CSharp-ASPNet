using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[ApiController]
[Route("api/[controller]")]
public class CitiesController: ControllerBase {
    private readonly AppDbContext _db;
    public CitiesController(AppDbContext db) => _db = db;
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        return Ok(await _db.Cities.ToListAsync());
    }
}