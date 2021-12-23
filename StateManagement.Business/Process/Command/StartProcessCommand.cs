using MediatR;
using StateManagement.Data.Repositories;
using StateManagement.Data.Repositories.EventStore;
using StateManagement.Data.Repositories.Mongo;
using StateManagement.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Task.Command
{
    public class StartProcessCommand : IRequest<Unit>
    {
        public Guid FlowId { get; set; }

        public Guid TaskId { get; set; }

        public StartProcessCommand(Guid flowId, Guid taskId)
        {
            FlowId = flowId;
            TaskId = taskId;
        }
    }

    public class StartProcessCommandHandler : IRequestHandler<StartProcessCommand, Unit>
    {
        private readonly EventStoreRepository _eventStoreRepository;
        private readonly IMongoRepository<FlowState> _mongoRepository;

        public StartProcessCommandHandler(EventStoreRepository eventStoreRepository, IMongoRepository<FlowState> mongoRepository)
        {
            _eventStoreRepository = eventStoreRepository;
            _mongoRepository = mongoRepository;
        }

        public async Task<Unit> Handle(StartProcessCommand command, CancellationToken cancellationToken)
        {
            var flowStates = _mongoRepository.Search<FlowState>(s => s.FlowId == command.FlowId)
                                               .Select(s => new FlowStateModel() { FlowId = s.FlowId, StateId = s.StateId, Order = s.Order })
                                               .ToList();
            if (flowStates.Count == 0)
            {
                throw new Exception("Flow'a ait state bulunamadı!");
            }

            flowStates.OrderBy(o => o.Order).First().ActiveState = true;

            var processAggregate = new ProcessAggregate();

            processAggregate.StartProcess(command.FlowId, command.TaskId, flowStates);

            await _eventStoreRepository.SaveAsync(processAggregate);

            return Unit.Value;
        }
    }
}
