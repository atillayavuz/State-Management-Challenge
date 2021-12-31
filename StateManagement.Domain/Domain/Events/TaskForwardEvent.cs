using StateManagement.Domain.BaseEntities;
using System;

namespace StateManagement.Domain.Model.StateManagement.Events
{
    public class TaskForwardEvent : EventBase
    {
        public Guid TaskId { get; set; }

        public TaskForwardEvent(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
