﻿using ContentManagementService.Core.Model;

namespace ContentManagementService.Data.Interface
{
    public interface ICommentServiceDataAccess
    {
        Task<Comment> FindCommentById(string commentId);
        Task<List<Comment>> FindCommentsByUserId(string userId);
        Task CreateComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(string commentId);
    }
}
