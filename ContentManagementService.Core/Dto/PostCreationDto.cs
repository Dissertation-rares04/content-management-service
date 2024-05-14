using ContentManagementService.Core.Model;

namespace ContentManagementService.Core.Dto
{
    public class PostCreationDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Category { get; set; }

        public List<Media> Medias { get; set; }
    }
}
