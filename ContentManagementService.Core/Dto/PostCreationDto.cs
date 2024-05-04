using ContentManagementService.Core.Model;

namespace ContentManagementService.Core.Dto
{
    public class PostCreationDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<string> Tags { get; set; }

        public List<Media> Medias { get; set; }
    }
}
