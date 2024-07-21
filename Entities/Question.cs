using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rankt.Entities.Common;

namespace Rankt.Entities;

public class Question : BaseAuditableEntity
{
    public Guid ExternalIdentifier { get; set; } = Guid.NewGuid();

    public required string Title { get; set; }
    public required QuestionStatus Status { get; set; }
    public required QuestionType Type { get; set; }
    public int? MaxSelectableItems { get; set; }

    public IList<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    public IList<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();
}

internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Navigation(x => x.Status)
            .IsRequired();

        builder.Navigation(x => x.Type)
            .IsRequired();

        builder.HasMany(x => x.Options)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasPrincipalKey(x => x.Id)
            .IsRequired();

        builder.HasMany(x => x.Responses)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId)
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
