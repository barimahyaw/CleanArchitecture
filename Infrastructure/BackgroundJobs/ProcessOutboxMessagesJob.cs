using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Persistence;
using Persistence.Outbox;
using Polly;
using Polly.Retry;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ApplicationDbContext dbContext, 
        IPublisher publisher,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var messages = await _dbContext
               .Set<OutboxMessage>()
               .Where(m => m.ProcessedDateUtc == null)
               .Take(20)
               .ToListAsync(context.CancellationToken);

            _logger.LogInformation("Processing {Count} outbox messages", messages.Count);

            foreach (var message in messages)
            {
                _logger.LogInformation("Processing outbox message {Id} of type {Type}", message.Id, message.Type);

                //try
                //{
                //    var domainEvent = JsonConvert
                //   .DeserializeObject<IDomainEvent>(message.Content);

                //    message.ProcessingAttempts++;

                //    if (domainEvent == null)
                //    {
                //        message.ProcessedDateUtc = DateTime.UtcNow;
                //        message.Error = "Failed to deserialize domain event";
                //        _logger.LogError("Failed to deserialize domain event for outbox message {Id} of type {Type}", message.Id, message.Type);
                //        continue;
                //    }

                //    await _publisher.Publish(domainEvent, context.CancellationToken);

                //    message.ProcessedDateUtc = DateTime.UtcNow;
                //    _logger.LogInformation("Successfully processed outbox message {Id} of type {Type}", message.Id, message.Type);
                //}
                //catch (Exception ex)
                //{
                //    message.ProcessLastAttemptOnUtc = DateTime.UtcNow;
                //    message.Error = ex.ToString();
                //    _logger.LogError(ex, "Failed to process outbox message {Id} of type {Type}", message.Id, message.Type);
                //}

                AsyncRetryPolicy policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(
                        3, 
                        //retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
                        retryAttempt => TimeSpan.FromMicroseconds(50 * retryAttempt));

                PolicyResult policyResult = await policy.ExecuteAndCaptureAsync(async () =>
                {
                    var domainEvent = JsonConvert
                        .DeserializeObject<IDomainEvent>(message.Content);

                    message.ProcessingAttempts++;

                    if (domainEvent == null)
                    {
                        message.ProcessedDateUtc = DateTime.UtcNow;
                        message.Error = "Failed to deserialize domain event";
                        _logger.LogError("Failed to deserialize domain event for outbox message {Id} of type {Type}", message.Id, message.Type);
                        return;
                    }

                    await _publisher.Publish(domainEvent, context.CancellationToken);

                    message.ProcessedDateUtc = DateTime.UtcNow;
                    _logger.LogInformation("Successfully processed outbox message {Id} of type {Type}", message.Id, message.Type);
                });
            }

            await _dbContext.SaveChangesAsync(context.CancellationToken);

            _logger.LogInformation("Successfully processed outbox messages");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process outbox messages");
        }
    }
}
