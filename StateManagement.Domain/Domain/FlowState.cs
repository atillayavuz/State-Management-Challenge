using StateManagement.Domain.Model;
using System;

namespace StateManagement.Domain
{
    public class FlowState : EntityBase<Guid>
    {
        public Guid FlowId { get; set; }

        public Guid StateId { get; set; }

        public int Order { get; set; }

        public FlowState(Guid id, Guid flowId, Guid stateId, int previousOrder = 0) : base(id)
        {
            FlowId = flowId;
            StateId = stateId;
            Order = previousOrder + 1;
        }
    }
}
