﻿using ContentManagementService.Business.Interface;
using ContentManagementService.Core;
using ContentManagementService.Core.CustomException;
using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Enum;
using ContentManagementService.Core.Model;
using ContentManagementService.Data.Interface;
using Newtonsoft.Json;

namespace ContentManagementService.Business.Implementation
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly ICommentServiceDataAccess _commentServiceDataAccess;
        private readonly IPostServiceDataAccess _postServiceDataAccess;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public CommentService(ICommentServiceDataAccess commentServiceDataAccess, IPostServiceDataAccess postServiceDataAccess, IRabbitMQProducer rabbitMQProducer, IUserResolver userResolver) : base(userResolver)
        {
            _commentServiceDataAccess = commentServiceDataAccess;
            _postServiceDataAccess = postServiceDataAccess;
            _rabbitMQProducer = rabbitMQProducer;
        }

        public async Task<List<Comment>> GetUserComments()
        {
            var result = await _commentServiceDataAccess.FindCommentsByUserId(_userResolver.UserId);

            return result;
        }

        public async Task CreateComment(CommentCreationDto commentCreationDto)
        {
            var comment = new Comment()
            {
                UserId = _userResolver.UserId,
                PostId = commentCreationDto.PostId,
                Content = commentCreationDto.Content
            };

            await _commentServiceDataAccess.CreateComment(comment);

            var post = await _postServiceDataAccess.FindPostById(commentCreationDto.PostId);
            var message = new
            {
                PostAuthorUserId = post.UserId,
                CommentAuthorUserId = _userResolver.UserId,
                PostId = post.Id,
                CommentContent = commentCreationDto.Content
            };
            _rabbitMQProducer.SendNotificationMessage(new Message { ActionType = ActionType.COMMENT_CREATED, Value = JsonConvert.SerializeObject(message) });
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
