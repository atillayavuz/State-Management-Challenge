using MediatR;
using StateManagement.Data.Repositories;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.State.Command
{
    public class CreateStateCommand : IRequest<Domain.State>
    {
        public string Name { get; set; }

        public CreateStateCommand(string name)
        {
            Name = name;
        }
    }

    public class CreateStateCommandHandler : IRequestHandler<CreateStateCommand, Domain.State>
    {
        private readonly IMongoRepository<Domain.State> _mongoRepository;

        public CreateStateCommandHandler(IMongoRepository<Domain.State> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Domain.State> Handle(CreateStateCommand command, CancellationToken cancellationToken)
        {
            var state = new Domain.State(Guid.NewGuid(), command.Name);

            await _mongoRepository.InsertOneAsync(state);

            return state;
        }
    }
}
