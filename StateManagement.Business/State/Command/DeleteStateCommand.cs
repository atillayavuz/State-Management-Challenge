using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.State.Command
{
    public class DeleteStateCommand : IRequest<Unit>
    {
        public Guid StateId { get; set; }

        public DeleteStateCommand(Guid stateId)
        {
            StateId = stateId;
        }
    }

    public class DeleteFlowCommandHandler : IRequestHandler<DeleteStateCommand, Unit>
    {
        private readonly IMongoRepository<Domain.State> _mongoRepository;

        public DeleteFlowCommandHandler(IMongoRepository<Domain.State> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Unit> Handle(DeleteStateCommand command, CancellationToken cancellationToken)
        {
            var state = await _mongoRepository.GetAsync<Domain.State>(g => g.Id == command.StateId.ToString());
            state.Delete();

            await _mongoRepository.UpdateOneAsync(d => d.Id == command.StateId.ToString(), state);

            return Unit.Value;
        }
    }
}
