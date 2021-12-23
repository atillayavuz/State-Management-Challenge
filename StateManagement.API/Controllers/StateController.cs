using MediatR;
using Microsoft.AspNetCore.Mvc;
using StateManagement.Business.State.Command;
using StateManagement.Business.State.Queries;
using System;
using System.Threading.Tasks;

namespace StateManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : Controller
    {
        private readonly IMediator _mediator;

        public StateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            return Ok(await _mediator.Send(new GetStateById(id)));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromQuery] string name)
        {
            var result = await _mediator.Send(new CreateStateCommand(name));

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _mediator.Send(new DeleteStateCommand(id));

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateStateCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}
