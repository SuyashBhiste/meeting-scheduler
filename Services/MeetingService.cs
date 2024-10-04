using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MeetingScheduler.Data;
using MeetingScheduler.Models;

namespace MeetingScheduler.Services;

public class MeetingService : IMeetingService
{
    private readonly ApplicationDbContext _context;

    public MeetingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Meeting> ScheduleMeetingAsync(DateTime startTime, DateTime endTime, int roomId, List<int> participantIds)
    {
        var roomExists = await _context.Rooms.AnyAsync(r => r.Id == roomId);
        if (!roomExists)
            throw new InvalidOperationException("The specified RoomId does not exist.");

        if (!await CheckAvailabilityAsync(startTime, endTime, roomId, participantIds))
            throw new InvalidOperationException("Scheduling conflict detected.");

        var meeting = new Meeting
        {
            StartTime = startTime,
            EndTime = endTime,
            RoomId = roomId,
            ParticipantMeetings = participantIds.Select(id => new ParticipantMeeting { ParticipantId = id }).ToList()
        };

        _context.Meetings.Add(meeting);
        await _context.SaveChangesAsync();

        return meeting;
    }

    public async Task<List<Meeting>> GetAllMeetingsAsync()
    {
        return await _context.Meetings
            .Include(m => m.Room)
            .Include(m => m.ParticipantMeetings).ThenInclude(pm => pm.Participant)
            .ToListAsync();
    }

    public async Task<bool> CheckAvailabilityAsync(DateTime startTime, DateTime endTime, int roomId, List<int> participantIds)
    {
        var roomConflict = await _context.Meetings
            .AnyAsync(m => m.RoomId == roomId &&
                           m.StartTime < endTime &&
                           m.EndTime > startTime);

        if (roomConflict) return false;

        foreach (var participantId in participantIds)
        {
            var participantConflict = await _context.ParticipantMeetings
                .AnyAsync(pm => pm.ParticipantId == participantId &&
                                pm.Meeting.StartTime < endTime &&
                                pm.Meeting.EndTime > startTime);

            if (participantConflict) return false;
        }

        return true;
    }
}
