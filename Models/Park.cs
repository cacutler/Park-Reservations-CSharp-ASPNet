public class Park {
    public int Id { get; set; }
    public int CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    // Store as JSON string: e.g., {"Monday":["09:00-12:00","13:00-17:00"],"Tuesday":[...]}
    public string Schedule { get; set; } = "{}";
    public City City { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}