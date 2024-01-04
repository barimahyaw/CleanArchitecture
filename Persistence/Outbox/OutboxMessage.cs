using Domain.Primitives;

namespace Persistence.Outbox;

public sealed class OutboxMessage : Entity
{
    //public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedDateUtc { get; set; }
    public DateTime? ProcessLastAttemptOnUtc { get; set; }
    public int ProcessingAttempts { get; set; }
    public string? Error { get; set; }
}