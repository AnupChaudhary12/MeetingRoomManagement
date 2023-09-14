
using MeetingRoomManagement2.Models.Domain;
using MeetingRoomManagement2.Models.DTo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DatabaseContext _databaseContext;
        public IActionResult Display()
        {
            return View();
        }


        public AdminController( UserManager<ApplicationUser> userManager,DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> GetRoom()
        {
            var rooms = await _databaseContext.Rooms.ToListAsync();
            return View(rooms);
        }

        public async Task<IActionResult> GetRoomDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var room = await _databaseContext.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        public IActionResult CreateRoom()
        {
            return View();
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateRoom(RoomModel room)
        {

            if (ModelState.IsValid)
            {
                _databaseContext.Add(room);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetRoom));
            }
            return View(room);
        }

        public async Task<IActionResult> EditRoom(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var room = await _databaseContext.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        [HttpPost]
        //  [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditRoom(int id, RoomModel room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _databaseContext.Update(room);
                    await _databaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomModelExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetRoom));
            }
            return View(room);
        }

        public async Task<IActionResult> DeleteRoom(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var room = await _databaseContext.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        [HttpPost, ActionName("DeleteRoom")]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _databaseContext.Rooms.FindAsync(id);
            _databaseContext.Rooms.Remove(room);
            await _databaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(GetRoom));
        }
        private bool RoomModelExists(int id)
        {
            return _databaseContext.Rooms.Any(e => e.Id == id);
        }

		public IActionResult BookRoom()
		{
			if (ViewBag.Rooms == null)
			{
				ViewBag.Rooms = _databaseContext.Rooms.ToList();
			}

			return View();
		}

        [HttpPost]
        public async Task<IActionResult> BookRoom(int RoomModelId, DateTime startTime, DateTime endTime)
        {
            var room = await _databaseContext.Rooms.
                            Include(r => r.Bookings)
                            .FirstOrDefaultAsync(m => m.Id == RoomModelId);
            
            if (room == null)
            {
                return NotFound("Room not found");
            }
            if (room.Status == " NotAvailable")
            {
                return NotFound("Room is unavailable for booking because Room Capacity is Full");
            }
            if (room.Bookings.Any(b => b.StartTime <= startTime && b.EndTime >= startTime) || room.Bookings.Any(b => b.StartTime <= endTime && b.EndTime >= endTime) || room.Bookings.Any(b => b.StartTime >= startTime && b.EndTime <= endTime))
            {
                return NotFound("Room is already booked during the selected time slot");
            }

            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User not found");
            }

            if (ViewBag.Rooms == null)
            {
                ViewBag.Rooms = _databaseContext.Rooms.ToList();
            }


            var booking = new BookingModel
            {
                StartTime = startTime,
                EndTime = endTime,
                UserID = userId,  // Assign the user's ID
                RoomModelId = RoomModelId,
            };

            _databaseContext.Bookings.Add(booking);
            await _databaseContext.SaveChangesAsync();

            return RedirectToAction(nameof(GetBookingDetails));
        }


        public async Task<IActionResult> BookingDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _databaseContext.Bookings.Where(m => m.RoomModelId == id).ToListAsync();
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        public async Task<IActionResult> GetBookingDetails()
        {
            var booking = await _databaseContext.Bookings.ToListAsync();
            return View(booking);
        }

		public async Task<IActionResult> DeleteBooking(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var booking = await _databaseContext.Bookings.FirstOrDefaultAsync(m => m.ID == id);
			if (booking == null)
			{
				return NotFound();
			}
			return View(booking);
		}
		[HttpPost, ActionName("DeleteBooking")]
		//  [ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteBookingConfirmed(int id)
		{

			var booking = await _databaseContext.Bookings.FindAsync(id);
			_databaseContext.Bookings.Remove(booking);
			await _databaseContext.SaveChangesAsync();
			return RedirectToAction(nameof(GetBookingDetails));
		}
        public async Task<IActionResult> EditBooking(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _databaseContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooking(int id, BookingModel booking)
        {
            if (id != booking.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _databaseContext.Update(booking);
                    await _databaseContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingModelExists(booking.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GetBookingDetails));
            }

            return View(booking);
        }
        public IActionResult AddParticipant(int bookingId)
        {
            var booking = _databaseContext.Bookings
                .Include(b => b.RoomModel)
                .FirstOrDefault(b => b.ID == bookingId);

            var room = booking?.RoomModel;

            if (room == null)
            {
                return NotFound();
            }

            var currentParticipantsCount = _databaseContext.Participants
                .Where(p => p.BookingId == bookingId)
                .Count();

            if (currentParticipantsCount >= room.Capacity)
            {
                return NotFound("Room is full");
            }

            var availableParticipants = _userManager.Users.Where(u => u != null).ToList();

            if (availableParticipants == null)
            {
                availableParticipants = new List<ApplicationUser>();
            }

            var viewModel = new AddParticipantViewModel
            {
                Booking = booking,
                AvailableParticipants = availableParticipants
            };

            ViewBag.AvailableParticipants = availableParticipants;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipant(AddParticipantViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                var participant = new ParticipantModel
                {
                    UserId = viewModel.UserId,
                    BookingId = viewModel.Booking.ID
                };
           
           _databaseContext.Participants.Add(participant);
                await _databaseContext.SaveChangesAsync();

                return RedirectToAction("GetParticipants", new { id = viewModel.Booking.ID });
            }

            return View(viewModel);
        }

		[HttpGet]
		public async Task<IActionResult> ListParticipantWithBookID(int? bookingId)
		{
			if (bookingId == null)
			{
				return NotFound();
			}

			var participants = await _databaseContext.Participants
				.Where(p => p.BookingId == bookingId)
				.ToListAsync();

			if (participants == null || participants.Count == 0)
			{
				return NotFound("There is no participant in this Booked Room");
			}

			return View(participants);
		}

		public async Task<IActionResult> GetParticipants()
        {
            var participants = await _databaseContext.Participants.ToListAsync();
            return View(participants);
        }
		public async Task<IActionResult> RemoveParticipant(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var participant = await _databaseContext.Participants.FirstOrDefaultAsync(m => m.Id == id);
			if (participant == null)
			{
				return NotFound();
			}
			return View(participant);
		}
		[HttpPost, ActionName("RemoveParticipant")]
		//  [ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveParticipantConfirmed(int id)
		{

			var participant = await _databaseContext.Participants.FindAsync(id);
			_databaseContext.Participants.Remove(participant);
			await _databaseContext.SaveChangesAsync();
			return RedirectToAction(nameof(GetParticipants));
		}




		private bool BookingModelExists(int id)
        {
			return _databaseContext.Bookings.Any(e => e.ID == id);
		}

	
	}
}
        
