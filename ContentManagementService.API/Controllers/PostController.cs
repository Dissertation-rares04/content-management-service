using ContentManagementService.Business.Interface;
using ContentManagementService.Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManagementService.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("my-posts")]
        public async Task<IActionResult> GetUserPosts()
        {
            try
            {
                var result = await _postService.GetUserPosts();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPostById([FromRoute] string id)
        {
            try
            {
                var result = await _postService.GetPostById(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendations()
        {
            try
            {
                var result = await _postService.GetRecommendations();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("categories-posts")]
        public async Task<IActionResult> GetCategoriesPosts()
        {
            try
            {
                var result = await _postService.GetCategoriesPosts();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] PostCreationDto postCreationDto)
        {
            try
            {
                await _postService.CreatePost(postCreationDto);

                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePost([FromBody] PostUpdationDto postUpdationDto)
        {
            try
            {
                var result = await _postService.UpdatePost(postUpdationDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        [Authorize("delete:post")]
        public async Task<IActionResult> DeletePost([FromBody] string postId)
        {
            try
            {
                var result = await _postService.DeletePost(postId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("like")]
        public async Task<IActionResult> LikePost([FromQuery] string postId)
        {
            try
            {
                var result = await _postService.InteractWithPost(postId, Core.Enum.InteractionType.LIKE);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("view")]
        public async Task<IActionResult> ViewPost([FromQuery] string postId)
        {
            try
            {
                var result = await _postService.InteractWithPost(postId, Core.Enum.InteractionType.VIEW);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
