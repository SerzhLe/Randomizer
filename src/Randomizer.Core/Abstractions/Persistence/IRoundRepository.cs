﻿using Randomizer.Domain.Entities;

namespace Randomizer.Application.Abstractions.Persistence;

public interface IRoundRepository
{
    Task<RoundEntity?> GetByIdAsync(Guid id);

    Task<RoundEntity> AddAsync(RoundEntity entity);

    Task UpdateAsync(RoundEntity entity);

    Task<List<RoundEntity>> GetAllByGameConfigId(Guid gameConfigId);
}
