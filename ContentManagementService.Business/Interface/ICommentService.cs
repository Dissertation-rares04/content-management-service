using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Model;

namespace ContentManagementService.Business.Interface
{
    public interface ICommentService
    {
        Task<List<Comment>> GetUserComments();
        Task CreateComment(CommentCreationDto commentCreationDto);
        Task<bool> UpdateComment(CommentUpdationDto commentUpdationDto);
        Task<bool> DeleteComment(CommentDeletionDto commentDeletionDto);
    }
}
