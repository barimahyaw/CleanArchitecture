﻿using Domain.Entities;

namespace Domain.Repositories;

public interface IGatheringRepository
{
    Task<Gathering?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Gathering gathering);
}
