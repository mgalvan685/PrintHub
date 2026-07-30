using PrintHub.Helpers;

namespace PrintHub.Database.Models;

public class BaseEntity
{
    public int Id { get; set; }

    public int? Legacy_Id { get; set; }

    public DateTime Created_On { get; set; }
    public DateTime? Updated_On { get; set; }

    public string Created_By { get; set; } = SystemUsersConstants.SYSTEM_USER;
    public string? Updated_By { get; set; }
}
