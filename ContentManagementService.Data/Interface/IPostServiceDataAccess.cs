using ContentManagementService.Core.Model;

namespace ContentManagementService.Data.Interface
{
    public interface IPostServiceDataAccess
    {
        Task<Post> FindPostById(string postId);
        Task<List<Post>> FindPostsByUserId(string userId);
        Task CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(string postId);
    }
}
