public record ParkDto(int Id, int CityId, string CityName, string Name, string Address, string Schedule) {
    public ParkDto(Park p): this(p.Id, p.CityId, p.City?.Name ?? "", p.Name, p.Address, p.Schedule) {}
}