using ContentManagementService.Core.Model;

namespace ContentManagementService.Core.Dto
{
    public class CategoryPosts
    {
        public string Category { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
