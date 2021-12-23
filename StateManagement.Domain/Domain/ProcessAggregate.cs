using StateManagement.Domain.Model.BaseEntities;
using StateManagement.Domain.Model.StateManagement.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateManagement.Domain
{
    public class FlowStateModel
    {
        public Guid FlowId { get; set; }

        public Guid StateId { get; set; }

        public int Order { get; set; }

        public bool ActiveState { get; set; }
    }

    public class ProcessAggregate : AggregateRoot
    {
        public Guid FlowId { get; set; }

        public List<FlowStateModel> FlowStates { get; set; }
        public ProcessAggregate() { }

        public void StartProcess(Guid flowId, Guid taskId, List<FlowStateModel> flowStates)
        {
            var @event = new ProcessCreateEvent(taskId, flowId, flowStates);

            Apply(@event);
        }

        public void MoveForward(Guid taskId)
        {
            var currentState = FlowStates.First(f => f.ActiveState == true);

            if (currentState == FlowStates.OrderBy(o => o.Order).Last())
            {
                throw new Exception($"Process already finished with TaskId {taskId}");
            }

            var @event = new TaskForwardEvent(taskId);

            Apply(@event);
        }

        public void MoveBackward(Guid taskId)
        {
            var currentState = FlowStates.First(f => f.ActiveState == true);

            if (currentState == FlowStates.OrderBy(o => o.Order).First())
            {
                throw new Exception($"Task is on starting state, TaskId : {taskId}, Cannot move back!");
            }

            var @event = new TaskBackwardEvent(taskId);

            Apply(@event);
        }

        public void TaskAssignToOrder(Guid taskId, int orderId)
        {
            var currentState = FlowStates.First(f => f.ActiveState == true);

            if (currentState.Order == orderId)
            {
                throw new Exception($"Task : {taskId} already in Order : {orderId}");
            }

            if (!FlowStates.Any(a => a.Order == orderId))
            {
                throw new Exception($"Order : {orderId} cannot found in this flow");
            }

            if (currentState.Order + 1 != orderId || currentState.Order - 1 != orderId)
            {
                throw new Exception($"Task : {taskId} cannot assign to Order : {orderId}");
            }

            var @event = new TaskAssignToOrderEvent(taskId, orderId);

            Apply(@event);
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case ProcessCreateEvent e: ProcessCreated(e); break;
                case TaskForwardEvent e: TaskForwardCreated(e); break;
                case TaskBackwardEvent e: TaskBackwardCreated(e); break;
                case TaskAssignToOrderEvent e: TaskAssignToOrder(e); break;
            }
        }

        private void ProcessCreated(ProcessCreateEvent @event)
        {
            AggregateId = @event.TaskId;
            FlowId = @event.FlowId;
            FlowStates = @event.FlowStates;
        }

        private void TaskForwardCreated(TaskForwardEvent @event)
        {
            var currentState = FlowStates.First(f => f.ActiveState == true);

            FlowStates.First(f => f.Order == currentState.Order + 1).ActiveState = true;

            FlowStates.First(f => f.ActiveState == true).ActiveState = false;
        }

        private void TaskBackwardCreated(TaskBackwardEvent @event)
        {
            var currentState = FlowStates.First(f => f.ActiveState == true);

            FlowStates.First(f => f.Order == currentState.Order - 1).ActiveState = true;

            FlowStates.First(f => f.ActiveState == true).ActiveState = false;
        }

        private void TaskAssignToOrder(TaskAssignToOrderEvent @event)
        {
            FlowStates.ForEach(f => f.ActiveState = false);

            FlowStates.First(f => f.Order == @event.OrderId).ActiveState = true;
        }
    }
}
