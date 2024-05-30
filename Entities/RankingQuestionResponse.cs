using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class RankingQuestionResponse : BaseEntity
{
    public int RankingQuestionId { get; set; }
    public required RankingQuestion RankingQuestion { get; set; }

    public Guid ExternalIdentifier { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public DateTime Created { get; set; }

    public IList<RankingQuestionResponseItem> ResponseItems { get; set; } = new List<RankingQuestionResponseItem>();
}

internal sealed class RankingQuestionResponseConfiguration : IEntityTypeConfiguration<RankingQuestionResponse>
{
    public void Configure(EntityTypeBuilder<RankingQuestionResponse> builder)
    {
        builder.Property(x => x.Username)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Created)
            .IsRequired();

        builder.HasMany(x => x.ResponseItems)
            .WithOne(x => x.RankingQuestionResponse)
            .HasForeignKey(x => x.RankingQuestionResponseId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasPrincipalKey(x => x.Id);

        builder.Navigation(x => x.ResponseItems)
            .AutoInclude();
    }
}
