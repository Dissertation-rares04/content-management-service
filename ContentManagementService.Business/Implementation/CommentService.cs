using ContentManagementService.Business.Interface;
using ContentManagementService.Core.CustomException;
using ContentManagementService.Core;
using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Model;
using ContentManagementService.Data.Implementation;
using ContentManagementService.Data.Interface;

namespace ContentManagementService.Business.Implementation
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly ICommentServiceDataAccess _commentServiceDataAccess;

        public CommentService(ICommentServiceDataAccess commentServiceDataAccess, IUserResolver userResolver) : base(userResolver)
        {
            _commentServiceDataAccess = commentServiceDataAccess;
        }

        public async Task<List<Comment>> GetUserComments()
        {
            var result = await _commentServiceDataAccess.FindCommentsByUserId(_userResolver.UserId);

            return result;
        }

        public async Task CreateComment(CommentCreationDto commentCreationDto)
        {
            var Comment = new Comment()
            {
                UserId = _userResolver.UserId,
                Content = commentCreationDto.Content,
                Likes = new List<Like>()
            };

            await _commentServiceDataAccess.CreateComment(Comment);
        }

        public async Task<bool> UpdateComment(CommentUpdationDto commentUpdationDto)
        {
            var comment = await _commentServiceDataAccess.FindCommentById(commentUpdationDto.Id);
            comment.Content = commentUpdationDto.Content;

            var result = await _commentServiceDataAccess.UpdateComment(comment);
            return result;
        }

        public async Task<bool> DeleteComment(CommentDeletionDto commentDeletionDto)
        {
            var comment = await _commentServiceDataAccess.FindCommentById(commentDeletionDto.Id);

            if (comment.UserId != _userResolver.UserId)
            {
                throw new BusinessException(ErrorCodes.CanOnlyDeletePersonalComments);
            }

            var result = await _commentServiceDataAccess.DeleteComment(commentDeletionDto.Id);

            return result;
        }
    }
}
