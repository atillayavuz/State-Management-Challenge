using MediatR;
using StateManagement.Data.Repositories;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.State.Command
{
    public class UpdateStateCommand : IRequest<Unit>
    {
        public Guid StateId { get; set; }

        public string Name { get; set; }

        public string Descripton { get; set; }

        public UpdateStateCommand(Guid stateId, string name, string description)
        {
            StateId = stateId;
            Name = name;
            Descripton = description;
        }
    }

    public class UpdateStateCommandHandler : IRequestHandler<UpdateStateCommand, Unit>
    {
        private readonly IMongoRepository<Domain.State> _mongoRepository;

        public UpdateStateCommandHandler(IMongoRepository<Domain.State> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Unit> Handle(UpdateStateCommand command, CancellationToken cancellationToken)
        {
            var state = await _mongoRepository.GetAsync<Domain.State>(g => g.Id == command.StateId.ToString());

            state.UpdateState(command.Name);

            await _mongoRepository.UpdateOneAsync(f => f.Id == command.StateId.ToString(), state);

            return Unit.Value;
        }
    }
}
