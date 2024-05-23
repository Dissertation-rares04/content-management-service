using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Enum;
using ContentManagementService.Core.Model;

namespace ContentManagementService.Business.Interface
{
    public interface ICommentService
    {
        Task<List<Comment>> GetUserComments();
        Task<List<Comment>> GetCommentsForPost(string postId);
        Task CreateComment(CommentCreationDto commentCreationDto);
        Task<bool> UpdateComment(CommentUpdationDto commentUpdationDto);
        Task<bool> DeleteComment(string commentId);
        Task<bool> InteractWithComment(string commentId, InteractionType interactionType);
    }
}
