
using MeetingRoomManagement2.Models.Domain;
using MeetingRoomManagement2.Models.DTo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MeetingRoomManagement.Controllers
{
	[Authorize]
	public class BookingController : Controller
	{
		
		private readonly DatabaseContext _databaseContext;
		public BookingController( DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
			
		}
		public IActionResult Display()
		{
			return View();
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
			var room = await _databaseContext.Rooms.FirstOrDefaultAsync(m => m.Id == RoomModelId);
			if (room == null)
			{
				return NotFound("Room not found");
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

			return RedirectToAction(nameof(BookRoom));
		}


		public async Task<IActionResult> BookingDetails(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var booking = await _databaseContext.Bookings.FirstOrDefaultAsync(m => m.RoomModelId == id);
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






	}
}
