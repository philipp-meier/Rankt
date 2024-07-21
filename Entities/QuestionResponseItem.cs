using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class QuestionResponseItem : BaseEntity
{
    public int QuestionResponseId { get; set; }
    public required QuestionResponse QuestionResponse { get; set; }

    public int QuestionOptionId { get; set; }
    public required QuestionOption QuestionOption { get; set; }

    public required int? Rank { get; set; }
}

/*
internal sealed class QuestionResponseItemConfiguration : IEntityTypeConfiguration<QuestionResponseItem>
{
    public void Configure(EntityTypeBuilder<QuestionResponseItem> builder)
    {
        builder.Property(x => x.Rank)
            .IsRequired();
    }
}
*/