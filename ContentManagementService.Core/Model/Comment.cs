﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentManagementService.Core.Model
{
    public class Comment : AuditableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public List<Interaction> Interactions { get; set; } = new List<Interaction>();

        public List<Interaction> Likes => Interactions.Where(x => x.InteractionType == Enum.InteractionType.LIKE).ToList();
    }
}
