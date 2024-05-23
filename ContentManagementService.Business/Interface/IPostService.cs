using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Enum;
using ContentManagementService.Core.Model;

namespace ContentManagementService.Business.Interface
{
    public interface IPostService
    {
        Task<List<Post>> GetUserPosts();
        Task<Post> GetPostById(string postId);
        Task<List<CategoryPosts>> GetCategoriesPosts();
        Task<List<Post>> GetRecommendations();
        Task CreatePost(PostCreationDto postCreationDto);
        Task<bool> UpdatePost(PostUpdationDto postUpdationDto);
        Task<bool> DeletePost(string postId);
        Task<bool> InteractWithPost(string postId, InteractionType interactionType);
    }
}
