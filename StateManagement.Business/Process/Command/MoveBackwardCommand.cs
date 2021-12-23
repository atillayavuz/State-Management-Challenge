using MediatR;
using StateManagement.Data.Repositories.EventStore;
using StateManagement.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Task.Command
{
    public class MoveBackwardCommand : IRequest<Unit>
    {
        public Guid TaskId { get; set; }

        public MoveBackwardCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }

    public class MoveBackwardCommandHandler : IRequestHandler<MoveBackwardCommand, Unit>
    {
        private readonly EventStoreRepository _eventStoreRepository;

        public MoveBackwardCommandHandler(EventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<Unit> Handle(MoveBackwardCommand command, CancellationToken cancellationToken)
        {
            var processAggregate = await _eventStoreRepository.LoadAsync<ProcessAggregate>(command.TaskId);

            processAggregate.MoveBackward(command.TaskId);

            await _eventStoreRepository.SaveAsync(processAggregate);

            return Unit.Value;
        }
    }
}
