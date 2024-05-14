using ContentManagementService.Business.Interface;
using ContentManagementService.Core;
using ContentManagementService.Core.CustomException;
using ContentManagementService.Core.Dto;
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

        public async Task<bool> DeletePost(PostDeletionDto postDeletionDto)
        {
            var post = await _postServiceDataAccess.FindPostById(postDeletionDto.Id);

            if (post.UserId != _userResolver.UserId)
            {
                throw new BusinessException(ErrorCodes.CanOnlyDeletePersonalPosts);
            }

            var result = await _postServiceDataAccess.DeletePost(postDeletionDto.Id);

            return result;
        }

        public async Task<bool> LikePost(string postId)
        {
            var like = new Like()
            {
                UserId = _userResolver.UserId,
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _postServiceDataAccess.LikePost(postId, like);

            var post = await _postServiceDataAccess.FindPostById(postId);
            await _kafkaProducer.ProduceLikeEvent(post, like);

            return result;
        }
    }
}
