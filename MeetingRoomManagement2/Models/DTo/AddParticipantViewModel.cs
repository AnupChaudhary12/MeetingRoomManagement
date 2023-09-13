using MeetingRoomManagement2.Models.Domain;

namespace MeetingRoomManagement2.Models.DTo
{
    public class AddParticipantViewModel
    {
        public BookingModel? Booking { get; set; }
        public string? UserId { get; set; }
        public List<ApplicationUser>? AvailableParticipants { get; set; }
    }
}
