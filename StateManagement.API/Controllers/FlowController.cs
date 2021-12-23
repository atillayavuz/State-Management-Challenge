using MediatR;
using Microsoft.AspNetCore.Mvc;
using StateManagement.Business.Flow.Command;
using StateManagement.Business.Flow.Queries;
using StateManagement.WebApi.Requests;
using System;
using System.Threading.Tasks;

namespace StateManagement.WebApi.Controllers
{
    public class FlowController : Controller
    {
        private readonly IMediator _mediator;
        public FlowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getflowstates")]
        public async Task<IActionResult> getflowstates([FromQuery] Guid id)
        {
            return Ok(await _mediator.Send(new GetFlowById(id)));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateFlowRequest request)
        {
            var result = await _mediator.Send(new CreateFlowCommand(request.Name, request.Description));

            return Ok(result);
        }

        [HttpPost("createFlowState")]
        public async Task<IActionResult> Create(CreateFlowStateRequest request)
        {
            var result = await _mediator.Send(new CreateFlowStateCommand(request.FlowId, request.StateId));

            return Ok(result);
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _mediator.Send(new DeleteFlowCommand(id));

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateFlowCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

    }
}
