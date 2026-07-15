namespace PrintHub.Database.Models;

public class BaseEntity
{
    public int Id { get; set; }

    public DateTime Created_On { get; set; }
    public DateTime? Updated_On { get; set; }

    public string Created_By { get; set; } = "system";
}
