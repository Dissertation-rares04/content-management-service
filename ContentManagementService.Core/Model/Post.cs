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

        public string Category { get; set; }

        public string Content { get; set; }

        public List<Media> Medias { get; set; } = new List<Media>();

        public List<Interaction> Interactions { get; set; } = new List<Interaction>();

        public List<Interaction> Likes => Interactions.Where(x => x.InteractionType == Enum.InteractionType.LIKE).ToList();

        public List<Interaction> Views => Interactions.Where(x => x.InteractionType == Enum.InteractionType.VIEW).ToList();
    }
}
