using System;
using System.Collections.Generic;
using System.Linq;

namespace StateManagement.Domain.Model.BaseEntities
{
    public abstract class AggregateRoot
    {
        readonly IList<object> _changes = new List<object>();

        public Guid AggregateId { get; protected set; } = Guid.Empty;

        public long Version { get; private set; } = -1;

        protected abstract void When(object @event);

        public void Apply(object @event)
        {
            Version++;
            When(@event);

            _changes.Add(@event);
        }

        public void Load(long version, IEnumerable<dynamic> history)
        {
            Version = version;

            foreach (var e in history)
            {
                When(e);
            }
        }

        public object[] GetChanges() => _changes.ToArray();
    }
}
