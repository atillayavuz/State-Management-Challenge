using StateManagement.Domain.BaseEntities;
using System;

namespace StateManagement.Domain.Model.StateManagement.Events
{
    public class TaskAssignToOrderEvent : EventBase
    {
        public Guid TaskId { get; set; }

        public int OrderId { get; set; }

        public TaskAssignToOrderEvent(Guid taskId, int orderId)
        {
            TaskId = taskId;
            OrderId = orderId;
        }
    }
}
