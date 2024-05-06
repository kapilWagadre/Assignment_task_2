using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Assignment_task_2.Models
{
    public class DataBaseConnect : IdentityDbContext
    {
        public DataBaseConnect(DbContextOptions<DataBaseConnect> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ground> Grounds { get; set; }
        public DbSet<TurnamentBooking> TurnamentBookings { get; set; }
        public DbSet<BookingModel> Bookingkmodel { get; set; }
    }
}
