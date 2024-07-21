using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rankt.Entities;
using Rankt.Entities.Common;
using Rankt.Infrastructure.Persistence;

namespace Rankt.Shared.Audit;

public class AuditableEntitySaveChangesInterceptor(
    IHttpContextAccessor httpContextAccessor,
    IServiceProvider serviceProvider)
    : BaseSaveChangesInterceptor
{
    protected override void UpdateEntities(DbContext context)
    {
        if (context is not ApplicationDbContext dbContext)
        {
            return;
        }

        var changedAuditableEntity = dbContext.ChangeTracker.Entries<BaseAuditableEntity>()
            .Where(x => x.State is not (EntityState.Detached or EntityState.Unchanged) && !IsAutomaticStatusChange(x))
            .ToArray();

        if (changedAuditableEntity.Length == 0)
        {
            return;
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var currentUser = userManager.GetUserAsync(httpContextAccessor.HttpContext!.User).Result;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException(nameof(AuditableEntitySaveChangesInterceptor));
        }

        var currentUtcDate = DateTime.UtcNow;
        foreach (var entry in changedAuditableEntity)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = currentUser;
                entry.Entity.Created = currentUtcDate;
            }

            entry.Entity.LastModifiedBy = currentUser;
            entry.Entity.LastModified = currentUtcDate;
        }
    }

    private static bool IsAutomaticStatusChange(EntityEntry<BaseAuditableEntity> entry)
    {
        if (entry.State == EntityState.Added || entry.Entity is not Question)
        {
            return false;
        }

        // TODO: Find a better way to ignore the automatic status change from "Published" -> "In Progress".
        var isAutomaticStatusChange = entry.Properties.All(x => !x.IsModified ||
                                                                (x.Metadata.Name == "StatusId" &&
                                                                 (int)x.OriginalValue! ==
                                                                 QuestionStatus.Published.Id &&
                                                                 (int)x.CurrentValue! ==
                                                                 QuestionStatus.InProgress.Id));

        return isAutomaticStatusChange;
    }
}
