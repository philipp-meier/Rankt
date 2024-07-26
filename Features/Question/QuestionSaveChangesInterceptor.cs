using Microsoft.EntityFrameworkCore;
using Rankt.Infrastructure.Persistence;
using Rankt.Shared;

namespace Rankt.Features.Question;

public class QuestionSaveChangesInterceptor
    : BaseSaveChangesInterceptor
{
    protected override void UpdateEntities(DbContext context)
    {
        if (context is not ApplicationDbContext dbContext)
        {
            return;
        }

        var changedQuestionEntries = dbContext.ChangeTracker.Entries<Entities.Question>()
            .Where(x => x.State is not (EntityState.Detached or EntityState.Unchanged))
            .ToArray();

        foreach (var entry in changedQuestionEntries)
        {
            QuestionService.EnsureOptionsOrdered(entry.Entity);
        }
    }
}
