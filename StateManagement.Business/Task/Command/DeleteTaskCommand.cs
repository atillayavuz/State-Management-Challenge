using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Task.Command
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public Guid TaskId { get; set; }

        public DeleteTaskCommand(Guid taskId)
        {
            TaskId = taskId;
        }
    }

    public class DeleteFlowCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly IMongoRepository<Domain.Task> _mongoRepository;

        public DeleteFlowCommandHandler(IMongoRepository<Domain.Task> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Unit> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _mongoRepository.GetAsync<Domain.Task>(g => g.Id == command.TaskId.ToString());
            task.Delete();

            await _mongoRepository.UpdateOneAsync(d => d.Id == command.TaskId.ToString(), task);

            return Unit.Value;
        }
    }
}
