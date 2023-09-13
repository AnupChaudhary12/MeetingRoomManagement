
using MeetingRoomManagement2.Models.Domain;
using MeetingRoomManagement2.Models.DTo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomManagement.Controllers
{
    public class RoomController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public RoomController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> GetRoom()
        {
            var rooms = await _databaseContext.Rooms.ToListAsync();
            return View(rooms);
        }

        public async Task<IActionResult> GetRoomDetails(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var room = await _databaseContext.Rooms.FirstOrDefaultAsync(m => m.Id == id);
            if (room==null)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom([Bind("Id,RoomName,Capacity")] RoomModel room)
        {
            
            if (ModelState.IsValid)
            {
                _databaseContext.Add(room);
                await _databaseContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetRoom));
            }
            return View(room);
        }

    }
}
