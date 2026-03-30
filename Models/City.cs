public class City {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string County { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public ICollection<Park> Parks { get; set; } = new List<Park>();
    public ICollection<ApplicationUser> Admins { get; set; } = new List<ApplicationUser>();
}