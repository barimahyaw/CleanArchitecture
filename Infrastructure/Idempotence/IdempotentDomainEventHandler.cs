using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Outbox;

namespace Infrastructure.Idempotence;

internal class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly ApplicationDbContext _dbContext;

    public IdempotentDomainEventHandler(
        INotificationHandler<TDomainEvent> decorated,
        ApplicationDbContext dbContext)
    {
        _decorated = decorated;
        _dbContext = dbContext;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        //string consumerName = _decorated.GetType().FullName!;
        string consumer = _decorated.GetType().Name;

        if (await _dbContext.Set<OutboxMessageConsumer>()
                .AnyAsync(
                    outboxMessageConsumer =>    
                        outboxMessageConsumer.Name == consumer && 
                        outboxMessageConsumer.Id == notification.Id, 
                    cancellationToken))
        {
            return;
        }

        await _decorated.Handle(notification, cancellationToken);.

        _dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

internal interface IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
}