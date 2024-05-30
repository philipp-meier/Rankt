using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class RankingQuestion : BaseAuditableEntity
{
    public Guid ExternalIdentifier { get; set; } = Guid.NewGuid();

    public required string Title { get; set; }
    public required RankingQuestionStatus Status { get; set; }
    public int? MaxSelectableItems { get; set; }

    public IList<RankingQuestionOption> Options { get; set; } = new List<RankingQuestionOption>();
    public IList<RankingQuestionResponse> Responses { get; set; } = new List<RankingQuestionResponse>();
}

internal sealed class RankingQuestionConfiguration : IEntityTypeConfiguration<RankingQuestion>
{
    public void Configure(EntityTypeBuilder<RankingQuestion> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(x => x.Options)
            .WithOne(x => x.RankingQuestion)
            .HasForeignKey(x => x.RankingQuestionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasPrincipalKey(x => x.Id)
            .IsRequired();

        builder.HasMany(x => x.Responses)
            .WithOne(x => x.RankingQuestion)
            .HasForeignKey(x => x.RankingQuestionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasPrincipalKey(x => x.Id)
            .IsRequired(false);

        builder.Navigation(x => x.Options)
            .AutoInclude();

        builder.Navigation(x => x.LastModifiedBy)
            .AutoInclude();

        builder.Navigation(x => x.CreatedBy)
            .AutoInclude();
    }
}
