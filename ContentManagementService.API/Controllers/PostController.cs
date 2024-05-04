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

        [HttpGet("read/my-posts")]
        public async Task<IActionResult> GetUserPosts()
        {
            try
            {
                await _postService.GetUserPosts();

                return Ok();
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
        public async Task<IActionResult> DeletePost([FromBody] PostDeletionDto postDeletionDto)
        {
            try
            {
                var result = await _postService.DeletePost(postDeletionDto);

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
