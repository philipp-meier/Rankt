using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class RankingQuestionResponseItem : BaseEntity
{
    public int RankingQuestionResponseId { get; set; }
    public required RankingQuestionResponse RankingQuestionResponse { get; set; }

    public int RankingQuestionOptionId { get; set; }
    public required RankingQuestionOption RankingQuestionOption { get; set; }

    public required int Rank { get; set; }
}

internal sealed class RankingQuestionResponseItemConfiguration : IEntityTypeConfiguration<RankingQuestionResponseItem>
{
    public void Configure(EntityTypeBuilder<RankingQuestionResponseItem> builder)
    {
        builder.Property(x => x.Rank)
            .IsRequired();
    }
}
