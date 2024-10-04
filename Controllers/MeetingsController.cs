using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeetingScheduler.Models;
using MeetingScheduler.Services;

namespace MeetingScheduler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeetingsController : ControllerBase
{
    private readonly IMeetingService _meetingService;

    public MeetingsController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    [HttpPost]
    public async Task<IActionResult> ScheduleMeeting([FromBody] ScheduleMeetingRequest request)
    {
        try
        {
            var meeting = await _meetingService.ScheduleMeetingAsync(
                request.StartTime,
                request.EndTime,
                request.RoomId,
                request.ParticipantIds
            );

            return CreatedAtAction(nameof(GetAllMeetings), new { id = meeting.Id }, meeting);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMeetings()
    {
        var meetings = await _meetingService.GetAllMeetingsAsync();
        return Ok(meetings);
    }
}

public class ScheduleMeetingRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int RoomId { get; set; }
    public List<int> ParticipantIds { get; set; }
}
