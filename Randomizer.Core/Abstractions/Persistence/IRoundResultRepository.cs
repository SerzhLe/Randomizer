﻿using Randomizer.Domain.Entities;

namespace Randomizer.Core.Abstractions.Persistence;
public interface IRoundResultRepository
{
    Task<RoundResultEntity> AddAsync(RoundResultEntity entity);

    Task UpdateAsync(RoundResultEntity entity);

    Task<RoundResultEntity?> FindAsync(Guid id);
}
