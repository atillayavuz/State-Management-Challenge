using EventStore.Client;
using Newtonsoft.Json;
using StateManagement.Domain.Model.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManagement.Data.Repositories.EventStore
{
    public class EventStoreRepository
    {
        private readonly EventStoreClient _eventStore;

        public EventStoreRepository(EventStoreClient eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task SaveAsync<T>(T aggregate) where T : AggregateRoot, new()
        {
            var events = aggregate.GetChanges()
                .Select(@event => new EventData(
                    Uuid.NewUuid(),
                    @event.GetType().Name,
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event.GetType().Name))
                    ))
                .ToArray();

            var originalVersion = aggregate.Version - events.Count();

            var streamName = GetStreamName(aggregate, aggregate.AggregateId);

            await _eventStore.AppendToStreamAsync(streamName, StreamRevision.FromInt64(originalVersion), events);
        }

        public async Task<T> LoadAsync<T>(Guid aggregateId, DateTime? createdDate = null) where T : AggregateRoot, new()
        {
            var aggregate = new T();
            var streamName = GetStreamName(aggregate, aggregateId);

            var result = _eventStore.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);

            List<ResolvedEvent> resolvedEvents;

            if (createdDate.HasValue)
            {
                resolvedEvents = await result.Where(w => w.Event.Created < createdDate).ToListAsync();
            }
            else
            {
                resolvedEvents = await result.ToListAsync();
            }

            if (!resolvedEvents.Any())
            {
                return aggregate;
            }

            //TODO : Deserialize etme işlem düzeltilmeli.
            aggregate.Load(resolvedEvents.Last().OriginalEvent.EventNumber.ToInt64(),
                           resolvedEvents.Select(@event => JsonConvert.DeserializeObject(Encoding.UTF8.GetString(@event.OriginalEvent.Data.ToArray()),
                                        GetType("StateManagement.Domain.Model.StateManagement.Events." + @event.OriginalEvent.EventType))).ToArray());

            return aggregate;
        }

        private string GetStreamName<T>(T type, Guid aggregateId) => $"{type.GetType().Name}-{aggregateId}";

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }

    }
}
