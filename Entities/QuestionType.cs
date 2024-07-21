using Rankt.Entities.Common;

namespace Rankt.Entities;

public class QuestionType : BaseEntity
{
    public static readonly QuestionType
        RankingQuestion = new() { Id = 1, Identifier = "RQ", Name = "Ranking Question" };

    public static readonly QuestionType Voting = new() { Id = 2, Identifier = "V", Name = "Voting" };

    public required string Identifier { get; set; }
    public required string Name { get; set; }
}
