using Microsoft.EntityFrameworkCore;
using MeetingScheduler.Models;

namespace MeetingScheduler.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<ParticipantMeeting> ParticipantMeetings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParticipantMeeting>()
            .HasKey(pm => new { pm.ParticipantId, pm.MeetingId });

        modelBuilder.Entity<ParticipantMeeting>()
            .HasOne(pm => pm.Participant)
            .WithMany(p => p.ParticipantMeetings)
            .HasForeignKey(pm => pm.ParticipantId);

        modelBuilder.Entity<ParticipantMeeting>()
            .HasOne(pm => pm.Meeting)
            .WithMany(m => m.ParticipantMeetings)
            .HasForeignKey(pm => pm.MeetingId);
    }
}
