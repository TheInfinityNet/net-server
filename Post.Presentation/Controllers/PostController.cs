using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Application.Helpers;

namespace InfinityNetServer.Services.Post.Presentation.Controllers
{
    [ApiController]
    public class PostController(
        IPostService _postService,
        IStringLocalizer<PostSharedResource> localizer) : ControllerBase
    {

        [HttpPost("create-post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var response = await _postService.CreatePost(request);
            return CreatedAtAction(nameof(GetPostById), new { id = response.Id }, response);
        }

        [HttpPut("update-post/{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostRequest request)
        {
<<<<<<< HEAD
            if (id != request.Id) return BadRequest("ID mismatch");

            var response = await _postService.UpdatePost(request);
            return Ok(response);
=======
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }
            await _postService.UpdatePost(request);
            return Ok(new CommonMessageResponse(
                localizer["post_updated_success", request.Id].ToString()
            ));
>>>>>>> 88f702cbae1aaa0fe95a72f508fe90f750c60d9e
        }

        [HttpDelete("delete-post/{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            await _postService.DeletePost(id);
            return Ok(new CommonMessageResponse(
                localizer["post_deleted_success", id].ToString()
            ));
        }

        [HttpGet("get-post/{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            Domain.Entities.Post response = await _postService.GetById(id);
            return Ok(PostHelper.ToResponse(response));
        }

        [HttpGet("all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            IEnumerable<Domain.Entities.Post> response = await _postService.GetAll();
            return Ok(PostHelper.ToResponses(response));
        }

        [HttpGet("profile/{ownerId}")]
        public async Task<IActionResult> GetPostsByOwnerId(string ownerId)
        {
            IEnumerable<Domain.Entities.Post> response = await _postService.GetAllByOwnerId(ownerId);
            return Ok(PostHelper.ToResponses(response));
        }

        [HttpGet("sub-posts/{parentId}")]
        public async Task<IActionResult> GetPostsByParentId(string parentId)
        {
            IEnumerable<Domain.Entities.Post> response = await _postService.GetAllByParentId(parentId);
            return Ok(PostHelper.ToResponses(response));
        }

        [HttpGet("group-post/{groupId}")]
        public async Task<IActionResult> GetPostsByGroupId(string groupId)
        {
            IEnumerable<Domain.Entities.Post> response = await _postService.GetAllByGroupId(groupId);
            return Ok(PostHelper.ToResponses(response));
        }
    }
}
