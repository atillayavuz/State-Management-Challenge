using MediatR;
using StateManagement.Data.Repositories.EventStore;
using StateManagement.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.State.Queries
{
    public class GetProcessLastState : IRequest<ProcessAggregate>
    {
        public Guid AggregateId { get; set; }

        public DateTime? EventCreatedAt { get; set; }

        public GetProcessLastState(Guid aggregateId, DateTime? eventCreatedAt = null)
        {
            AggregateId = aggregateId;
            EventCreatedAt = eventCreatedAt;
        }
    }

    public class GetProcessLastStateHandler : IRequestHandler<GetProcessLastState, ProcessAggregate>
    {
        private readonly EventStoreRepository _eventStoreRepository;

        public GetProcessLastStateHandler(EventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<ProcessAggregate> Handle(GetProcessLastState command, CancellationToken cancellationToken)
        {
            return await _eventStoreRepository.LoadAsync<ProcessAggregate>(command.AggregateId, command.EventCreatedAt);
        }
    }
}
