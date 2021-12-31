using System;

namespace StateManagement.Domain.BaseEntities
{
    public class EventBase
    {
        public DateTime CreatedAt { get; set; }

        public EventBase()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
