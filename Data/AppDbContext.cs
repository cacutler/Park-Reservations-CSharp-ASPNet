using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser> {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<City> Cities => Set<City>();
    public DbSet<Park> Parks => Set<Park>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().HasOne(u => u.AdminForCity).WithMany(c => c.Admins).HasForeignKey(u => u.AdminForCityId).OnDelete(DeleteBehavior.SetNull);
        builder.Entity<Park>().HasOne(p => p.City).WithMany(c => c.Parks).HasForeignKey(p => p.CityId);
        builder.Entity<Reservation>().HasOne(r => r.User).WithMany(u => u.Reservations).HasForeignKey(r => r.UserId);
        builder.Entity<Reservation>().HasOne(r => r.Park).WithMany(p => p.Reservations).HasForeignKey(r => r.ParkId);
    }
}