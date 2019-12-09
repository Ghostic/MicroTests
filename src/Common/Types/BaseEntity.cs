using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Types
{
    public abstract class BaseEntity : IIdentifiable
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; }

        public BaseEntity()
        {
            Id = ObjectId.GenerateNewId().ToString();
            CreatedDate = DateTime.UtcNow;
            SetUpdatedDate();
        }

        protected virtual void SetUpdatedDate()
            => UpdatedDate = DateTime.UtcNow;
    }
}