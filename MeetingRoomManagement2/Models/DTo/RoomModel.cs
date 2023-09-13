using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement2.Models.DTo
{
    public class RoomModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string RoomName { get; set; }
        public int Capacity { get; set; }

        public string? Status { get; set; }
		 public ICollection<BookingModel>? Bookings { get; set; }



	}
}
