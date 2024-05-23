using ContentManagementService.Business.Interface;
using ContentManagementService.Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContentManagementService.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("my-comments")]
        public async Task<IActionResult> GetUserComments()
        {
            try
            {
                var result = await _commentService.GetUserComments();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("comments/{postId}")]
        public async Task<IActionResult> GetCommentsForPost([FromRoute] string postId)
        {
            try
            {
                var result = await _commentService.GetCommentsForPost(postId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreationDto commentCreationDto)
        {
            try
            {
                await _commentService.CreateComment(commentCreationDto);

                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentUpdationDto commentUpdationDto)
        {
            try
            {
                var result = await _commentService.UpdateComment(commentUpdationDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete")]
        [Authorize("delete:comment")]
        public async Task<IActionResult> DeleteComment([FromQuery] string commentId)
        {
            try
            {
                var result = await _commentService.DeleteComment(commentId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("like")]
        public async Task<IActionResult> LikeComment([FromQuery] string commentId)
        {
            try
            {
                var result = await _commentService.InteractWithComment(commentId, Core.Enum.InteractionType.LIKE);

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
