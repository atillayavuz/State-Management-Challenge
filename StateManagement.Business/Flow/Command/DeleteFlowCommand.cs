using MediatR;
using StateManagement.Data.Repositories;
using StateManagement.Data.Repositories.Mongo;
using StateManagement.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Flow.Command
{
    public class DeleteFlowCommand : IRequest<Unit>
    {
        public Guid FlowId { get; set; }

        public DeleteFlowCommand(Guid flowId)
        {
            FlowId = flowId;
        }
    }

    public class DeleteFlowCommandHandler : IRequestHandler<DeleteFlowCommand, Unit>
    {
        private readonly IMongoRepository<FlowState> _mongoRepository;

        public DeleteFlowCommandHandler(IMongoRepository<FlowState> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Unit> Handle(DeleteFlowCommand command, CancellationToken cancellationToken)
        {
            var state = await _mongoRepository.GetAsync<Domain.Flow>(g => g.Id == command.FlowId.ToString());
            state.Delete();

            await _mongoRepository.UpdateOneAsync(d => d.Id == command.FlowId.ToString(), state);

            return Unit.Value;
        }
    }
}
