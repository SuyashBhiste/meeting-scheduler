using System.Collections.Generic;

namespace MeetingScheduler.Models;

public class Participant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ParticipantMeeting> ParticipantMeetings { get; set; }
}
