using HotelBookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Data
{
    public class HotelDbContext : DbContext {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }
        public DbSet<Hotel> Hotels => Set<Hotel>();
    }
}
