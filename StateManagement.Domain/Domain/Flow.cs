using StateManagement.Domain.Model;
using System;

namespace StateManagement.Domain
{
    public class Flow : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string Descripton { get; set; }


        public Flow(Guid id, string name, string description = null) : base(id)
        {
            Name = name;
            Descripton = description;
        }

        public void UpdateFlow(string name, string description = null)
        {
            Name = name;
            Descripton = description;
            Update();
        }
    }
}
