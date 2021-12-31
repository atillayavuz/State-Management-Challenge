using StateManagement.Domain.BaseEntities;
using System;

namespace StateManagement.Domain.Model.StateManagement.Events
{
    public class TaskBackwardEvent : EventBase
    {
        public Guid TaskId { get; set; }

        public TaskBackwardEvent(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
