using Microsoft.AspNetCore.Identity;
public class ApplicationUser : IdentityUser {
    public string Name { get; set; } = string.Empty;
    // Null means regular user; a value means they're admin for that city
    public int? AdminForCityId { get; set; }
    public City? AdminForCity { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}