using MediatR;
using StateManagement.Data.Repositories.Mongo;
using StateManagement.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Flow.Command
{
    public class CreateFlowStateCommand : IRequest<FlowState>
    {
        public Guid FlowId { get; set; }

        public Guid StateId { get; set; }


        public CreateFlowStateCommand(Guid flowId, Guid stateId)
        {
            FlowId = flowId;
            StateId = stateId;
        }
    }

    public class CreateFlowStateCommandHandler : IRequestHandler<CreateFlowStateCommand, FlowState>
    {
        private readonly IMongoRepository<FlowState> _mongoRepository;

        public CreateFlowStateCommandHandler(IMongoRepository<FlowState> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<FlowState> Handle(CreateFlowStateCommand command, CancellationToken cancellationToken)
        {
            var flowState = _mongoRepository.Search<FlowState>(f => f.FlowId == command.FlowId).OrderByDescending(o => o.Order).FirstOrDefault();

            var newFlowState = new FlowState(Guid.NewGuid(), command.FlowId, command.StateId, flowState != null ? flowState.Order : 0);

            await _mongoRepository.InsertOneAsync(newFlowState);

            return newFlowState;
        }
    }
}
