using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MeetingScheduler.Data;
using MeetingScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingScheduler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] RoomDTO roomDto)
    {
        if (roomDto == null || string.IsNullOrEmpty(roomDto.Name))
        {
            return BadRequest("Room name is required.");
        }

        // Check if a room with the same name already exists
        var existingRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.Name.ToLower() == roomDto.Name.ToLower());

        if (existingRoom != null)
        {
            return Conflict(new { message = "A room with this name already exists." });
        }

        var room = new Room
        {
            Name = roomDto.Name
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _context.Rooms.ToListAsync();
        return Ok(rooms);
    }
}
