using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace StateManagement.Domain.Model
{
    public interface IEntityBase<T>
    {
        T EntityId { get; }
    }

    public class EntityBase<T> : IEntityBase<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; protected set; }

        public T EntityId { get; protected set; }

        public DateTime CreateAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public EntityBase(T id)
        {
            Id = id.ToString();
            EntityId = id;
            CreateAt = DateTime.UtcNow;
        }

        public void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            DeletedAt = DateTime.UtcNow;
        }
    }
}
