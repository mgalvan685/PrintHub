using PrintHub.Database.Models;
using PrintHub.Helpers;

namespace PrintHub.Services;

public abstract class BaseService
{
    protected void SetCreatedFields(BaseEntity entity, string? user = null)
    {
        entity.Created_On = DateTime.UtcNow;
        entity.Created_By = user ?? SystemUsersConstants.SYSTEM_USER;
    }

    protected void SetUpdatedFields(BaseEntity entity, string? user = null)
    {
        entity.Updated_On = DateTime.UtcNow;
        entity.Updated_By = user ?? SystemUsersConstants.SYSTEM_USER;
    }
}
