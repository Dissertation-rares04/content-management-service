using ContentManagementService.Core.AppSettings;
using ContentManagementService.Core.Model;
using ContentManagementService.Data.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ContentManagementService.Data.Implementation
{
    public class CommentServiceDataAccess : BaseServiceDataAccess, ICommentServiceDataAccess
    {
        protected readonly IMongoCollection<Comment> _commentCollection;

        public CommentServiceDataAccess(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {
            _commentCollection = _mongoDatabase.GetCollection<Comment>(mongoDbSettings.Value.CommentCollectionName);
        }

        public async Task<Comment> FindCommentById(string commentId)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.Id, commentId);

            var result = await _commentCollection.FindAsync<Comment>(filter);

            return result.SingleOrDefault();
        }

        public async Task<List<Comment>> FindCommentsByUserId(string userId)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.UserId, userId);

            var result = await _commentCollection.FindAsync<Comment>(filter);

            return result.ToList();
        }

        public async Task CreateComment(Comment comment)
        {
            comment.CreatedAt = DateTime.Now;
            comment.UpdatedAt = null;

            await _commentCollection.InsertOneAsync(comment);
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            comment.UpdatedAt = DateTime.Now;

            var filter = Builders<Comment>.Filter.Eq(x => x.Id, comment.Id);

            var result = await _commentCollection.ReplaceOneAsync(filter, comment);

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteComment(string commentId)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.Id, commentId);

            var result = await _commentCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
    }
}
