using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class QuestionOption : BaseAuditableEntity
{
    public int QuestionId { get; set; }
    public Question Question { get; set; }

    public Guid ExternalIdentifier { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? Description { get; set; }
}

internal sealed class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
{
    public void Configure(EntityTypeBuilder<QuestionOption> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired(false);
    }
}
