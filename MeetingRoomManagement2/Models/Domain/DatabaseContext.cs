using MeetingRoomManagement2.Models.DTo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement2.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<ParticipantModel> Participants { get; set; }
    }
}
