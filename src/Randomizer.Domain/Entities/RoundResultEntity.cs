using Randomizer.Domain.Common;

namespace Randomizer.Domain.Entities;

public class RoundResultEntity : IEntity
{
    public Guid Id { get; set; }

    public double? Score { get; set; }

    public string? Comment { get; set; }

    public Guid WhoPerformActionId { get; set; }

    public Guid WhoPerformFeedbackId { get; set; }

    public Guid MessageId { get; set; }

    public Guid RoundId { get; set; }

    public ParticipantEntity? WhoPerformAction { get; set; }

    public ParticipantEntity? WhoPerformFeedback { get; set; }

    public MessageEntity? Message { get; set; }

    public RoundResultEntity? Round { get; set; }
}
