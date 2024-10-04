using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeetingScheduler.Models;

namespace MeetingScheduler.Services;

public interface IMeetingService
{
    Task<Meeting> ScheduleMeetingAsync(DateTime startTime, DateTime endTime, int roomId, List<int> participantIds);
    Task<List<Meeting>> GetAllMeetingsAsync();
    Task<bool> CheckAvailabilityAsync(DateTime startTime, DateTime endTime, int roomId, List<int> participantIds);
}
