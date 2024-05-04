using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Model;

namespace ContentManagementService.Business.Interface
{
    public interface IPostService
    {
        Task<List<Post>> GetUserPosts();
        Task CreatePost(PostCreationDto postCreationDto);
        Task<bool> UpdatePost(PostUpdationDto postUpdationDto);
        Task<bool> DeletePost(PostDeletionDto postDeletionDto);
    }
}
