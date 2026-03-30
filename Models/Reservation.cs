public class Reservation {
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ParkId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public ApplicationUser User { get; set; } = null!;
    public Park Park { get; set; } = null!;
}
public enum ReservationStatus {
    Pending,
    Approved,
    Denied
}