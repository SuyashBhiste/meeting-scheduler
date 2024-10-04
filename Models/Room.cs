using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeetingScheduler.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public ICollection<Meeting> Meetings { get; set; }
}
