using Domain.Entities;

namespace Domain.Abstractions;

public interface IMemberRepository
{
    void Add(Member member);
    Task<Member?> GetByIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
}