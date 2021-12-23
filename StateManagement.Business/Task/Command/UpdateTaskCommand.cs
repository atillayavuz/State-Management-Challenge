using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Task.Command
{
    public class UpdateTaskCommand : IRequest<Unit>
    {
        public Guid TaskId { get; set; }

        public string Name { get; set; }

        public string Descripton { get; set; }

        public UpdateTaskCommand(Guid taskId, string name, string description)
        {
            TaskId = taskId;
            Name = name;
            Descripton = description;
        }
    }

    public class UpdateFlowCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly IMongoRepository<Domain.Task> _mongoRepository;

        public UpdateFlowCommandHandler(IMongoRepository<Domain.Task> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Unit> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _mongoRepository.GetAsync<Domain.Task>(g => g.Id == command.TaskId.ToString());

            task.UpdateTask(command.Name, command.Descripton);

            await _mongoRepository.UpdateOneAsync<Domain.Task>(f => f.Id == command.TaskId.ToString(), task);

            return Unit.Value;
        }
    }
}
