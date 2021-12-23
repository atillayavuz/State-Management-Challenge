using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Flow.Command
{
    public class UpdateFlowCommand : IRequest<Unit>
    {
        public Guid FlowId { get; set; }

        public string Name { get; set; }

        public string Descripton { get; set; }

        public UpdateFlowCommand(Guid flowId, string name, string description)
        {
            FlowId = flowId;
            Name = name;
            Descripton = description;
        }
    }

    public class UpdateFlowCommandHandler : IRequestHandler<UpdateFlowCommand, Unit>
    {
        private readonly IMongoRepository<Domain.Flow> _mongoRepository;

        public UpdateFlowCommandHandler(IMongoRepository<Domain.Flow> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Unit> Handle(UpdateFlowCommand command, CancellationToken cancellationToken)
        {
            var flow = await _mongoRepository.GetAsync<Domain.Flow>(g => g.Id == command.FlowId.ToString());

            flow.UpdateFlow(command.Name, command.Descripton);

            await _mongoRepository.UpdateOneAsync(f => f.Id == command.FlowId.ToString(), flow);

            return Unit.Value;
        }
    }
}
