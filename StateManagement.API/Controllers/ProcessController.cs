using MediatR;
using Microsoft.AspNetCore.Mvc;
using StateManagement.Business.State.Queries;
using StateManagement.Business.Task.Command;
using StateManagement.WebApi.Requests;
using System;
using System.Threading.Tasks;

namespace StateManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : Controller
    {
        private readonly IMediator _mediator;

        public ProcessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetProcessLastState")]
        public async Task<IActionResult> GetProcessLastState([FromQuery] Guid id)
        {
            return Ok(await _mediator.Send(new GetProcessLastState(id)));
        }

        [HttpGet("GetProcessStateByDate")]
        public async Task<IActionResult> GetProcessStateByDate([FromQuery] Guid id, DateTime eventCreatedAt)
        {
            return Ok(await _mediator.Send(new GetProcessLastState(id, eventCreatedAt)));
        }

        [HttpPost("StartProcess")]
        public async Task<IActionResult> StartProcess(StartProcessRequest command)
        {
            await _mediator.Send(new StartProcessCommand(command.FlowId, command.TaskId));

            return Ok();
        }

        [HttpPost("MoveForward")]
        public async Task<IActionResult> MoveForward([FromQuery] Guid id)
        {
            await _mediator.Send(new MoveForwardCommand(id));

            return Ok();
        }

        [HttpPost("MoveBackward")]
        public async Task<IActionResult> MoveBackward([FromQuery] Guid id)
        {
            await _mediator.Send(new MoveBackwardCommand(id));

            return Ok();
        }
    }
}
