using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.State.Queries
{
    public class GetStateById : IRequest<Domain.State>
    {
        public Guid StateId { get; set; }

        public GetStateById(Guid stateId)
        {
            StateId = stateId;
        }
    }

    public class GetStateByIdHandler : IRequestHandler<GetStateById, Domain.State>
    {
        private readonly IMongoRepository<Domain.State> _mongoRepository;

        public GetStateByIdHandler(IMongoRepository<Domain.State> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Domain.State> Handle(GetStateById command, CancellationToken cancellationToken)
        {
            return await _mongoRepository.GetAsync<Domain.State>(g => g.Id == command.StateId.ToString());
        }
    }
}
