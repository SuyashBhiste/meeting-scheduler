using System;
using System.Collections.Generic;

namespace MeetingScheduler.Models;

public class Meeting
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public ICollection<ParticipantMeeting> ParticipantMeetings { get; set; }
}
