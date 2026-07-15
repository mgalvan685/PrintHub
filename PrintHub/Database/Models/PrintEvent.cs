namespace PrintHub.Database.Models;

public class PrintEvent : BaseEntity
{
    public int Project_ID { get; set; }
    public Project Project { get; set; } = null!;

    public string Event_Type { get; set; } = null!; // Queued, Printing, Completed, Failed
    public DateTime Timestamp { get; set; }
    public string? Notes { get; set; }
}
