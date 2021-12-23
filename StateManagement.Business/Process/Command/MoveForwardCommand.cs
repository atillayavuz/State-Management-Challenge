using MediatR;
using StateManagement.Data.Repositories.EventStore;
using StateManagement.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Task.Command
{
    public class MoveForwardCommand : IRequest<Unit>
    {
        public Guid TaskId { get; set; }

        public MoveForwardCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }

    public class TaskForwardCommandHandler : IRequestHandler<MoveForwardCommand, Unit>
    {
        private readonly EventStoreRepository _eventStoreRepository;

        public TaskForwardCommandHandler(EventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<Unit> Handle(MoveForwardCommand command, CancellationToken cancellationToken)
        {
            var processAggregate = await _eventStoreRepository.LoadAsync<ProcessAggregate>(command.TaskId);

            processAggregate.MoveForward(command.TaskId);

            await _eventStoreRepository.SaveAsync(processAggregate);

            return Unit.Value;
        }
    }
}
