using MeetingRoomManagement2.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomManagement2.Models.DTo
{
    public class ParticipantModel
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("BookingModel")]
        public int? BookingId { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]

        public string? UserId { get; set; }
        
    }
}
