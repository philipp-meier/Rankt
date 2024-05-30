using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class RankingQuestionOption : BaseAuditableEntity
{
    public int RankingQuestionId { get; set; }
    public RankingQuestion RankingQuestion { get; set; }

    public Guid ExternalIdentifier { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? Description { get; set; }
}

internal sealed class RankingQuestionOptionConfiguration : IEntityTypeConfiguration<RankingQuestionOption>
{
    public void Configure(EntityTypeBuilder<RankingQuestionOption> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired(false);
    }
}
