using System;

namespace StateManagement.Domain.Model.StateManagement.Events
{
    public class TaskForwardEvent
    {
        public Guid TaskId { get; set; }

        public TaskForwardEvent(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
