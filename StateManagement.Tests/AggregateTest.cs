using StateManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StateManagement.Tests
{
    public class AggregateTest
    {
        private Guid _taskId;

        private Guid _flowId;

        private List<FlowStateModel> _flowStates;

        public AggregateTest()
        {
            _taskId = Guid.NewGuid();
            _flowId = Guid.NewGuid();
            _flowStates = new List<FlowStateModel>();
            _flowStates.Add(new FlowStateModel() { FlowId = _flowId, StateId = Guid.NewGuid(), Order = 1, ActiveState = true });
            _flowStates.Add(new FlowStateModel() { FlowId = _flowId, StateId = Guid.NewGuid(), Order = 2, ActiveState = false });
            _flowStates.Add(new FlowStateModel() { FlowId = _flowId, StateId = Guid.NewGuid(), Order = 3, ActiveState = false });
        }

        [Fact]
        public void Should_Start_Process()
        {
            var process = new ProcessAggregate();
            process.StartProcess(_flowId, _taskId, _flowStates);

            Assert.Equal(_flowId, process.FlowId);
            Assert.Equal(_taskId, process.AggregateId);
            Assert.True(_flowStates.First(f => f.Order == 1).ActiveState);
        }

        [Fact]
        public void Throws_With_Process_Already_Finished_With_TaskId()
        {
            var process = new ProcessAggregate();
            process.StartProcess(_flowId, _taskId, _flowStates);
            process.MoveForward(_taskId);
            process.MoveForward(_taskId); // at the end of the flow.

            Assert.Throws<Exception>(() => process.MoveForward(_taskId));
        }

        [Fact]
        public void Throws_Process_Already_On_Starting_Point_With_TaskId()
        {
            var process = new ProcessAggregate();
            process.StartProcess(_flowId, _taskId, _flowStates);

            Assert.Throws<Exception>(() => process.MoveBackward(_taskId));
        }

        [Fact]
        public void Throws_Task_Already_In_Order()
        {
            var process = new ProcessAggregate();
            process.StartProcess(_flowId, _taskId, _flowStates);
            process.MoveForward(_taskId);
            process.MoveForward(_taskId);

            Assert.Throws<Exception>(() => process.TaskAssignToOrder(_taskId, 3));
        }

        [Fact]
        public void Throws_Order_Cannot_Found_In_This_Flows()
        {
            var process = new ProcessAggregate();
            process.StartProcess(_flowId, _taskId, _flowStates); 

            Assert.Throws<Exception>(() => process.TaskAssignToOrder(_taskId, 5));
        }

        [Fact]
        public void Throws_Task_Assign_To_Order()
        { 
            var process = new ProcessAggregate();
            process.StartProcess(_flowId, _taskId, _flowStates);
            process.MoveForward(_taskId);
            process.MoveForward(_taskId);

            Assert.Throws<Exception>(() => process.TaskAssignToOrder(_taskId, 1));
        } 
    }
}
