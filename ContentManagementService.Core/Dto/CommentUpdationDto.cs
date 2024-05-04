using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentManagementService.Core.Dto
{
    public class CommentUpdationDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Content { get; set; }
    }
}
