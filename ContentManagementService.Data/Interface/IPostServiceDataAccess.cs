using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Model;

namespace ContentManagementService.Data.Interface
{
    public interface IPostServiceDataAccess
    {
        Task<Post> FindPostById(string postId);
        Task<List<Post>> FindPostsByUserId(string userId);
        Task<List<CategoryPosts>> GetCategoriesPosts(string userId);
        Task<List<Post>> GetRecommendations(string userId);
        Task CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(string postId);
        Task<bool> SaveInteraction(string postId, Interaction interaction);
    }
}
