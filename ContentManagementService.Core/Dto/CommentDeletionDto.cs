using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentManagementService.Core.Dto
{
    public class CommentDeletionDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
