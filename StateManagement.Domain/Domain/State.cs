using StateManagement.Domain.Model;
using System;

namespace StateManagement.Domain
{
    public class State : EntityBase<Guid>
    {
        public string Name { get; set; }

        public State(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public void UpdateState(string name)
        {
            Name = name;
            Update();
        }
    }
}
