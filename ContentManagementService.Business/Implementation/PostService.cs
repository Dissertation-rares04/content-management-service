using ContentManagementService.Business.Interface;
using ContentManagementService.Core;
using ContentManagementService.Core.CustomException;
using ContentManagementService.Core.Dto;
using ContentManagementService.Core.Enum;
using ContentManagementService.Core.Model;
using ContentManagementService.Data.Interface;

namespace ContentManagementService.Business.Implementation
{
    public class PostService : BaseService, IPostService
    {
        private readonly IPostServiceDataAccess _postServiceDataAccess;

        private readonly IKafkaProducer _kafkaProducer;

        public PostService(IPostServiceDataAccess postServiceDataAccess, IKafkaProducer kafkaProducer, IUserResolver userResolver) : base(userResolver)
        {
            _postServiceDataAccess = postServiceDataAccess;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<List<Post>> GetUserPosts()
        {
            var result = await _postServiceDataAccess.FindPostsByUserId(_userResolver.UserId);

            return result;
        }

        public async Task<Post> GetPostById(string postId)
        {
            var result = await _postServiceDataAccess.FindPostById(postId);

            var view = new Interaction()
            {
                UserId = _userResolver.UserId,
                CreatedAt = DateTime.UtcNow,
                InteractionType = Core.Enum.InteractionType.VIEW
            };

            await _kafkaProducer.ProduceInteractionEvent(result, view);

            return result;
        }

        public async Task<List<CategoryPosts>> GetCategoriesPosts()
        {
            var result = await _postServiceDataAccess.GetCategoriesPosts(_userResolver.UserId);

            return result;
        }

        public async Task<List<Post>> GetRecommendations()
        {
            var result = await _postServiceDataAccess.GetRecommendations(_userResolver.UserId);

            return result;
        }

        public async Task CreatePost(PostCreationDto postCreationDto)
        {
            var post = new Post()
            {
                UserId = _userResolver.UserId,
                Title = postCreationDto.Title,
                Content = postCreationDto.Content,
                Category = postCreationDto.Category,
                Medias = postCreationDto.Medias
            };

            await _postServiceDataAccess.CreatePost(post);
        }

        public async Task<bool> UpdatePost(PostUpdationDto postUpdationDto)
        {
            var post = await _postServiceDataAccess.FindPostById(postUpdationDto.Id);
            post.Title = postUpdationDto.Title;
            post.Content = postUpdationDto.Content;
            post.Medias = postUpdationDto.Medias;

            var result = await _postServiceDataAccess.UpdatePost(post);
            return result;
        }

        public async Task<bool> DeletePost(string postId)
        {
            var post = await _postServiceDataAccess.FindPostById(postId);

            if (post.UserId != _userResolver.UserId)
            {
                throw new BusinessException(ErrorCodes.CanOnlyDeletePersonalPosts);
            }

            var result = await _postServiceDataAccess.DeletePost(postId);

            return result;
        }

        public async Task<bool> InteractWithPost(string postId, InteractionType interactionType)
        {
            var interaction = new Interaction()
            {
                UserId = _userResolver.UserId,
                CreatedAt = DateTime.UtcNow,
                InteractionType = interactionType
            };

            var result = await _postServiceDataAccess.SaveInteraction(postId, interaction);

            var post = await _postServiceDataAccess.FindPostById(postId);
            await _kafkaProducer.ProduceInteractionEvent(post, interaction);

            return result;
        }
    }
}
