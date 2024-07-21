using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class QuestionResponse : BaseEntity
{
    public int QuestionId { get; set; }
    public required Question Question { get; set; }

    public Guid ExternalIdentifier { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public DateTime Created { get; set; }

    public IList<QuestionResponseItem> ResponseItems { get; set; } = new List<QuestionResponseItem>();
}

internal sealed class QuestionResponseConfiguration : IEntityTypeConfiguration<QuestionResponse>
{
    public void Configure(EntityTypeBuilder<QuestionResponse> builder)
    {
        builder.Property(x => x.Username)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Created)
            .IsRequired();

        builder.HasMany(x => x.ResponseItems)
            .WithOne(x => x.QuestionResponse)
            .HasForeignKey(x => x.QuestionResponseId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasPrincipalKey(x => x.Id);

        builder.Navigation(x => x.ResponseItems)
            .AutoInclude();
    }
}
