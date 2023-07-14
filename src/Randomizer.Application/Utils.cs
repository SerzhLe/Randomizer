using Randomizer.Application.DTOs;
using Randomizer.Domain.Entities;

namespace Randomizer.Application;

public static class Utils
{
    public static List<WinnerDto> DefineWinners(List<RoundEntity> rounds, List<ParticipantEntity> participants)
    {
        var participantsScores = rounds
            .SelectMany(x => x.RoundResults.Select(x => new { x.Score, x.WhoPerformActionId }))
            .Where(x => x.Score.HasValue)
            .GroupBy(x => x.WhoPerformActionId)
            .Select(x => new WinnerDto
            {
                Id = x.Key,
                TotalScore = x.Sum(y => y.Score!.Value),
                NickName = participants.SingleOrDefault(y => y.Id == x.Key)?.NickName
            })
            .ToList();

        var winners = new List<WinnerDto>();

        if (participantsScores.Any())
        {
            var highestScore = participantsScores.MaxBy(x => x.TotalScore)!.TotalScore;

            winners = participantsScores.Where(x => x.TotalScore == highestScore).ToList();
        }

        return winners;
    }
}
