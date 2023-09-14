
using MeetingRoomManagement2.Models.Domain;
using MeetingRoomManagement2.Models.DTo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MeetingRoomManagement.Controllers
{
	[Authorize]
	public class BookingController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly DatabaseContext _databaseContext;
		public BookingController( DatabaseContext databaseContext, UserManager<ApplicationUser> userManager)
		{
			_databaseContext = databaseContext;
			_userManager = userManager;
			
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

        [HttpGet]
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

            var currentUser = await _userManager.GetUserAsync(User);
            if (booking.UserID != currentUser.UserName)
            {
                return RedirectToAction(nameof(Unauthorized));
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

            var existingBooking = await _databaseContext.Bookings.FindAsync(id);

            if (existingBooking == null)
            {
                return NotFound();
            }

            //var currentUser = await _userManager.GetUserAsync(User);
            //if (existingBooking.UserID != currentUser.UserName)
            //{
            //    return RedirectToAction(nameof(Unauthorized));
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    existingBooking.StartTime = booking.StartTime;
                    existingBooking.EndTime = booking.EndTime;


                    _databaseContext.Update(existingBooking);
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


        public IActionResult Unauthorized()
        {
            return View();
        }
        private bool BookingModelExists(int id)
        {
            return _databaseContext.Bookings.Any(e => e.ID == id);
        }
    }
}





	
