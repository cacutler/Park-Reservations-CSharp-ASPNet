public record ReservationDto(int Id, string ParkName, string CityName, DateOnly Date, TimeOnly Time, string Name, string Email, string PhoneNumber, ReservationStatus Status) {
    public ReservationDto(Reservation r) : this(r.Id, r.Park?.Name ?? "", r.Park?.City?.Name ?? "", r.Date, r.Time, r.Name, r.Email, r.PhoneNumber, r.Status) {}
}