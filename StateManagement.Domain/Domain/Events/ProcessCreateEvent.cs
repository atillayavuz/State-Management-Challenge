using System;
using System.Collections.Generic;

namespace StateManagement.Domain.Model.StateManagement.Events
{
    public class ProcessCreateEvent
    {
        public Guid FlowId { get; set; }

        public Guid TaskId { get; set; }

        public List<FlowStateModel> FlowStates { get; set; }

        public ProcessCreateEvent(Guid taskId, Guid flowId, List<FlowStateModel> flowStates)
        {
            TaskId = taskId;
            FlowId = flowId;
            FlowStates = flowStates;
        }
    }
}
