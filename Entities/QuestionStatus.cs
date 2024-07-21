using Rankt.Entities.Common;

namespace Rankt.Entities;

public class QuestionStatus : BaseEntity
{
    /// <summary>
    ///     Survey was just created and is only view- and editable by the creator/admin.
    /// </summary>
    public static readonly QuestionStatus New = new() { Id = 1, Identifier = "NEW", Name = "new" };

    /// <summary>
    ///     Survey was published and can not be edited anymore. Users can find the survey with its invitation ID.
    /// </summary>
    public static readonly QuestionStatus Published = new() { Id = 2, Identifier = "PUBLISHED", Name = "published" };

    /// <summary>
    ///     At least one user already responded to the survey.
    /// </summary>
    public static readonly QuestionStatus InProgress =
        new() { Id = 3, Identifier = "INPROGRESS", Name = "in progress" };

    /// <summary>
    ///     The survey was closed by the creator/admin. Users can now request the result of the survey.
    /// </summary>
    public static readonly QuestionStatus Completed = new() { Id = 4, Identifier = "COMPLETED", Name = "completed" };

    /// <summary>
    ///     The survey was archived by the creator/admin. Users cannot reach it anymore with their invitation ID.
    /// </summary>
    public static readonly QuestionStatus
        Archived = new() { Id = 5, Identifier = "ARCHIVED", Name = "archived" };

    public required string Identifier { get; set; }
    public required string Name { get; set; }
}
