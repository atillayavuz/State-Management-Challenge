using MediatR;
using StateManagement.Data.Repositories;
using StateManagement.Data.Repositories.Mongo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateManagement.Business.Task.Queries
{
    public class GetTaskById : IRequest<Domain.Task>
    {
        public Guid TaskId { get; set; }

        public GetTaskById(Guid taskId)
        {
            TaskId = taskId;
        }
    }

    public class GetTaskByIdHandler : IRequestHandler<GetTaskById, Domain.Task>
    {
        private readonly IMongoRepository<Domain.Task> _mongoRepository;

        public GetTaskByIdHandler(IMongoRepository<Domain.Task> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<Domain.Task> Handle(GetTaskById command, CancellationToken cancellationToken)
        {
            return await _mongoRepository.GetAsync<Domain.Task>(g => g.Id == command.TaskId.ToString());
        }
    }
}
