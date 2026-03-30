public class CityAdminAuthService {
    private readonly AppDbContext _db;
    public CityAdminAuthService(AppDbContext db) => _db = db;
    // Returns the city ID the user admins, or null if they're not an admin
    public async Task<int?> GetAdminCityId(string userId) {
        var user = await _db.Users.FindAsync(userId);
        return user?.AdminForCityId;
    }
    public async Task<bool> IsAdminForCity(string userId, int cityId) {
        var adminCityId = await GetAdminCityId(userId);
        return adminCityId == cityId;
    }
}