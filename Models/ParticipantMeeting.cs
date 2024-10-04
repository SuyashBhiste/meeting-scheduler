namespace MeetingScheduler.Models;

public class ParticipantMeeting
{
    public int ParticipantId { get; set; }
    public Participant Participant { get; set; }
    public int MeetingId { get; set; }
    public Meeting Meeting { get; set; }
}
