using ContentManagementService.Core.AppSettings;
using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Model;
using ContentManagementService.Data.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Interaction = ContentManagementService.Core.Model.Interaction;

namespace ContentManagementService.Data.Implementation
{
    public class PostServiceDataAccess : BaseServiceDataAccess, IPostServiceDataAccess
    {
        protected readonly IMongoCollection<Post> _postCollection;
        protected readonly IMongoCollection<UserRecommendation> _userRecommendationCollection;

        public PostServiceDataAccess(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {
            _postCollection = _mongoDatabase.GetCollection<Post>(mongoDbSettings.Value.PostCollectionName);
            _userRecommendationCollection = _mongoDatabase.GetCollection<UserRecommendation>(mongoDbSettings.Value.UserRecommendationCollectionName);
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

            var sort = Builders<Post>.Sort.Descending(x => x.CreatedAt);

            var result = await _postCollection
                .Find(filter)
                .Sort(sort)
                .ToListAsync();

            return result;
        }

        public async Task<List<CategoryPosts>> GetCategoriesPosts(string userId) 
        {
            var query = _postCollection.AsQueryable()
                .Where(x => x.UserId != userId)
                .OrderByDescending(x => x.CreatedAt)
                .GroupBy(x => x.Category)
                .Select(group => new CategoryPosts
                {
                    Category = group.Key,
                    Posts = group.Take(5).ToList()
                });

            var result = query.ToList();

            return result;
        }

        public async Task<List<Post>> GetRecommendations(string userId)
        {
            var filter = Builders<UserRecommendation>.Filter.Eq(x => x.UserId, userId);

            var result = await _userRecommendationCollection.FindAsync<UserRecommendation>(filter);

            var postsFilter = Builders<Post>.Filter.In(x => x.Id, result.ToEnumerable().Select(x => x.PostId));

            var postsResult = await _postCollection.FindAsync<Post>(postsFilter);

            return postsResult.ToEnumerable().ToList();
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

        public async Task<bool> SaveInteraction(string postId, Interaction interaction)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.Id, postId);

            var update = Builders<Post>.Update.Push("Interactions", interaction);

            var result = await _postCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}
