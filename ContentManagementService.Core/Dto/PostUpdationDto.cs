using ContentManagementService.Core.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentManagementService.Core.Dto
{
    public class PostUpdationDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public List<Media> Medias { get; set; }
    }
}
