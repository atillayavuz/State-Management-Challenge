using System;

namespace StateManagement.Domain.Model.StateManagement.Events
{
    public class TaskBackwardEvent
    {
        public Guid TaskId { get; set; }

        public TaskBackwardEvent(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
