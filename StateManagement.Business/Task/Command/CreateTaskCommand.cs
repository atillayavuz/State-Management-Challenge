using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Task.Command
{
    public class CreateTaskCommand : IRequest<Domain.Task>
    {
        public string Name { get; set; }

        public string Descripton { get; set; }

        public CreateTaskCommand(string name, string description)
        {
            Name = name;
            Descripton = description;
        }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Domain.Task>
    {

        private readonly IMongoRepository<Domain.Task> _mongoRepository;

        public CreateTaskCommandHandler(IMongoRepository<Domain.Task> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Domain.Task> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var task = new Domain.Task(Guid.NewGuid(), command.Name, command.Descripton);

            await _mongoRepository.InsertOneAsync(task);

            return task;
        }
    }
}
