using ContentManagementService.Core.AppSettings;
using ContentManagementService.Core.Model;
using ContentManagementService.Data.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ContentManagementService.Data.Implementation
{
    public class PostServiceDataAccess : BaseServiceDataAccess, IPostServiceDataAccess
    {
        protected readonly IMongoCollection<Post> _postCollection;

        public PostServiceDataAccess(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {
            _postCollection = _mongoDatabase.GetCollection<Post>(mongoDbSettings.Value.PostCollectionName);
        }

        public async Task<Post> FindPostById(string postId)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, postId);

            var result = await _postCollection.FindAsync<Post>(filter);

            return result.SingleOrDefault();
        }

        public async Task<List<Post>> FindPostsByUserId(string userId)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.UserId, userId);

            var result = await _postCollection.FindAsync<Post>(filter);

            return result.ToList();
        }

        public async Task CreatePost(Post post)
        {
            post.CreatedAt = DateTime.Now;
            post.UpdatedAt = null;

            await _postCollection.InsertOneAsync(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            post.UpdatedAt = DateTime.Now;

            var filter = Builders<Post>.Filter.Eq(x => x.Id, post.Id);

            var result = await _postCollection.ReplaceOneAsync(filter, post);

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeletePost(string postId)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, postId);

            var result = await _postCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        public async Task<bool> LikePost(string postId, Like like)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, postId);

            var update = Builders<Post>.Update.Push("Likes", like);

            var result = await _postCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}
