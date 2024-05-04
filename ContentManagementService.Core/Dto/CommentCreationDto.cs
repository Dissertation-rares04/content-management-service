using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentManagementService.Core.Dto
{
    public class CommentCreationDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        public string Content { get; set; }
    }
}
