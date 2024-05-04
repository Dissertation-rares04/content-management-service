using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentManagementService.Core.Model
{
    public class Post : AuditableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public List<string> Tags { get; set; }
        public List<Media> Medias { get; set; }
        public List<Like> Likes { get; set; }
    }
}
