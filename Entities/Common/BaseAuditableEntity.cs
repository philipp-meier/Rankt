using Microsoft.AspNetCore.Identity;

namespace Rankt.Entities.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }
    public IdentityUser CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    public IdentityUser LastModifiedBy { get; set; }
}
