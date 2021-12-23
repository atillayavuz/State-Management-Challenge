using MediatR;
using Microsoft.AspNetCore.Mvc;
using StateManagement.Business.Task.Command;
using StateManagement.Business.Task.Queries;
using StateManagement.WebApi.Requests;
using System;
using System.Threading.Tasks;

namespace StateManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            return Ok(await _mediator.Send(new GetTaskById(id)));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateTaskRequest request)
        {
            var result = await _mediator.Send(new CreateTaskCommand(request.Name, request.Description));

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _mediator.Send(new DeleteTaskCommand(id));

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateTaskCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

    }
}
