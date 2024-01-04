﻿namespace Domain.Primitives;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    protected AggregateRoot(Guid id) 
        : base(id) 
    { 
    }

    protected AggregateRoot() { }

    protected void RaiseDomainEvent(IDomainEvent domainEvent) 
        => _domainEvents.Add(domainEvent);
    //{
    //    _domainEvents.Add(domainEvent);
    //}

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() 
        => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() 
        => _domainEvents.Clear();
}