using Shortify.Domain.Abstractions;
using Shortify.Domain.Abstractions.Repository;

namespace Shortify.Infrastructure;

internal class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
}
