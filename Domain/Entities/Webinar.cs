using Domain.Primitives;

namespace Domain.Entities;

public sealed class Webinar : Entity
{
    public Webinar(Guid id, string name, DateTime schedule)
        : base(id)
    {
        Name = name;
        Schedule = schedule;
    }

    public string Name { get; private set; }
    public DateTime Schedule { get; private set; }
}