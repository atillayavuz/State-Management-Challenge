using StateManagement.Domain.Model.BaseEntities;
using System;
using System.Threading.Tasks;

namespace StateManagement.Data.Repositories.EventStore
{
    public interface IEventStoreRepository
    {
        Task SaveAsync<T>(T aggregate) where T : AggregateRoot, new();

        Task<T> LoadAsync<T>(Guid aggregateId) where T : AggregateRoot, new();
    }
}
