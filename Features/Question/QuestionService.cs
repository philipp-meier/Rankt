namespace Rankt.Features.Question;

public static class QuestionService
{
    public static void EnsureOptionsOrdered(Entities.Question question)
    {
        var options = question.Options
            .OrderBy(x => x.Position)
            .ToArray();

        for (var i = 0; i < options.Length; i++)
        {
            options[i].Position = i;
        }
    }
}
