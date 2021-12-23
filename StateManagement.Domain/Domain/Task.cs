using StateManagement.Domain.Model;
using System;

namespace StateManagement.Domain
{
    public class Task : EntityBase<Guid>
    {
        public string Name { get; set; }

        public string Descripton { get; set; }

        public Task(Guid id, string name, string description = null) : base(id)
        {
            Name = name;
            Descripton = description;
        }

        public void UpdateTask(string name, string description = null)
        {
            Name = name;
            Descripton = description;
            Update();
        }
    }
}
