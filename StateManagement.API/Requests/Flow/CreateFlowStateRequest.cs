using System;

namespace StateManagement.WebApi.Requests
{
    public class CreateFlowStateRequest
    {
        public Guid FlowId { get; set; }

        public Guid StateId { get; set; }

    }
}
