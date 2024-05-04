using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentManagementService.Core.Dto
{
    public class PostDeletionDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
