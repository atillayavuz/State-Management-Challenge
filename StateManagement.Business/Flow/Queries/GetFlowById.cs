using MediatR;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Flow.Queries
{
    public class GetFlowById : IRequest<List<Domain.FlowState>>
    {
        public Guid FlowId { get; set; }

        public GetFlowById(Guid flowId)
        {
            FlowId = flowId;
        }
    }

    public class GetFlowByIdHandler : IRequestHandler<GetFlowById, List<Domain.FlowState>>
    {
        private readonly IMongoRepository<Domain.FlowState> _mongoRepository;

        public GetFlowByIdHandler(IMongoRepository<Domain.FlowState> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<List<Domain.FlowState>> Handle(GetFlowById command, CancellationToken cancellationToken)
        {
            return await _mongoRepository.SearchAsync<Domain.FlowState>(g => g.FlowId == command.FlowId);
        }
    }
}
