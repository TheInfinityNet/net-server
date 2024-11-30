using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Comment.Application;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using InfinityNetServer.Services.Comment.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Presentation.Controllers
{
    [Tags("Comment APIs")]
    [ApiController]
    public class CommentController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CommentController> logger,
        IStringLocalizer<CommentSharedResource> localizer,
        IMapper mapper,
        IMessageBus messageBus,
        ICommentService commentService,
        CommonFileClient commonFileClient,
        CommonProfileClient commonProfileClient) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Get comments of post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetByPostId(string postId, [FromQuery] string cursor = null, [FromQuery] int limit = 10)
        {
            //var response = await commentService.GetByPostId(request);

            //if (response.Comments == null || response.Comments.Count == 0)
            //    return NotFound(new { message = "No comments found for the given post ID." });

            return Ok();
        }

        [EndpointDescription("Get replied comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{parentId}/replies")]
        public async Task<IActionResult> GetChildComments(string parentId, [FromQuery] string cursor = null, [FromQuery] int limit = 10)
        {
            //if (commentId == Guid.Empty)
            //{
            //    return BadRequest("Parent Comment ID is required.");
            //}

            //var childComments = await commentService.GetChildCommentsAsync(commentId);

            //var profilesTasks = childComments.Select(async comment =>
            //{
            //    try
            //    {
            //        var profileResponse = await commonProfileClient.GetProfile(comment.ProfileId.ToString());

            //        comment.Profile = new SimpleProfileResponse
            //        {
            //            Username = profileResponse.Name,
            //            AvatarId = profileResponse.Avatar?.Id.ToString()
            //        };
            //    }
            //    catch (BaseException ex)
            //    {
            //        comment.Profile = null;
            //    }
            //    return comment;
            //});

            //await Task.WhenAll(profilesTasks);

            return Ok();
        }

        [EndpointDescription("Create a new comment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        [HttpPost("/")]
        public async Task<ActionResult> Create([FromBody] CommentBaseRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            Domain.Entities.Comment comment = mapper.Map<Domain.Entities.Comment>(request);
            comment.ProfileId = currentProfileId;

            var response = await commentService.Create(comment);

            if (request.FileMetadataId != null)
                await commentService.ConfirmSave(
                    response.Id.ToString(),
                    currentProfileId.ToString(),
                    comment.FileMetadataId.ToString(), messageBus);

            return Created(string.Empty, new
            {
                id = response.Id,
            });
        }

        [EndpointDescription("Delete a comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var comment = await commentService.GetById(id) 
                ?? throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);

            var response = await commentService.SoftDelete(id);

            return Ok(new
            {
                id = response.Id,
            });
        }

        [EndpointDescription("Update a comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(string id, [FromBody] CommentBaseRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                            : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var existedComment = await commentService.GetById(id)
                ?? throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);

            Domain.Entities.Comment comment = mapper.Map<Domain.Entities.Comment>(request);
            comment.Id = existedComment.Id;
            comment.ProfileId = currentProfileId;

            var response = await commentService.Update(comment);
            if (request.FileMetadataId != null)
                await commentService.ConfirmSave(
                    response.Id.ToString(),
                    currentProfileId.ToString(),
                    comment.FileMetadataId.ToString(), messageBus);

            return Ok(new
            {
                id = response.Id,
            });
        }

    }
}
