using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Flow.Command
{
    public class CreateFlowCommand : IRequest<Domain.Flow>
    {
        public string Name { get; set; }

        public string Descripton { get; set; }

        public CreateFlowCommand(string name, string description)
        {
            Name = name;
            Descripton = description;
        }
    }

    public class CreateFlowCommandHandler : IRequestHandler<CreateFlowCommand, Domain.Flow>
    {
        private readonly IMongoRepository<Domain.Flow> _mongoRepository;

        public CreateFlowCommandHandler(IMongoRepository<Domain.Flow> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Domain.Flow> Handle(CreateFlowCommand command, CancellationToken cancellationToken)
        {
            var flow = new Domain.Flow(Guid.NewGuid(), command.Name, command.Descripton);

            await _mongoRepository.InsertOneAsync(flow);

            return flow;
        }
    }
}
