using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Application.Helpers;
using InfinityNetServer.Services.Post.Application.IServices;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Profile.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Presentation.Controllers
{
    [Tags("Post APIs")]
    [ApiController]
    public class PostController(
        IAuthenticatedUserService authenticatedUserService,
        IPostService postService,
        IMapper mapper,
        ILogger<PostController> logger,
        IStringLocalizer<PostSharedResource> localizer) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Create a post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        [HttpPost("/")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostBaseRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId();

            postService.ValidateMediaPostType(request);

            postService.ValidateAudienceType(request.Audience);

            var post = mapper.Map<Domain.Entities.Post>(request);
            post.OwnerId = currentProfileId;

            if (request.Aggregates != null && request.Aggregates.Count > 0)
            {
                var subPosts = new List<Domain.Entities.Post>();
                
                foreach (var subPostDto in request.Aggregates)
                {
                    logger.LogInformation("Create multi-media post");
                    postService.ValidateMediaPostType(subPostDto);
                    var subPost = mapper.Map<Domain.Entities.Post>(subPostDto);
                    subPost.Audience = null;
                    subPost.OwnerId = currentProfileId;
                    subPosts.Add(subPost);
                }
                post.SubPosts = subPosts;
            }

            var response = await postService.Create(post);

            return Created(string.Empty, new
            {
                id = response.Id,
            });
        }

        //[EndpointDescription("Create a share post")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[Authorize]
        //[HttpPost("share")]
        //public async Task<IActionResult> CreateSharePost([FromBody] CreateSharePostRequest request)
        //{
        //    Guid currentProfileId = GetCurrentProfileId();

        //    postService.ValidateAudienceType(request.Audience);
        //    _ = await postService.GetById(request.ShareId)
        //        ?? throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);

        //    var post = mapper.Map<Domain.Entities.Post>(request);
        //    post.OwnerId = currentProfileId;

        //    var response = await postService.Create(post);
        //    return Created(string.Empty, new
        //    {
        //        id = response.Id,
        //    });
        //}

        //[EndpointDescription("Create a media post")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[Authorize]
        //[HttpPost("media")]
        //public async Task<IActionResult> CreateMediaPost([FromBody] CreateMediaPostRequest request)
        //{
        //    Guid currentProfileId = GetCurrentProfileId();

        //    postService.ValidateMediaPostType(request);

        //    postService.ValidateAudienceType(request.Audience);

        //    var post = mapper.Map<Domain.Entities.Post>(request);
        //    post.OwnerId = currentProfileId;

        //    logger.LogInformation("Create media post: {0}", post);

        //    var response = await postService.Create(post);

        //    await postService.ConfirmSave(
        //        response.Id.ToString(),
        //        post.OwnerId.ToString(),
        //        post.FileMetadataId.ToString());

        //    return Created(string.Empty, new
        //    {
        //        id = response.Id,
        //    });
        //}

        //[EndpointDescription("Create a multi-media post")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[Authorize]
        //[HttpPost("multi-media")]
        //public async Task<IActionResult> CreateMultiMediaPost([FromBody] CreateMultiMediaPostRequest request)
        //{
        //    Guid currentProfileId = GetCurrentProfileId();

        //    postService.ValidateAudienceType(request.Audience);

        //    var post = mapper.Map<Domain.Entities.Post>(request);
        //    post.OwnerId = currentProfileId;

        //    var subPosts = new List<Domain.Entities.Post>();
        //    foreach (var subPostDto in request.Aggregates)
        //    {
        //        postService.ValidateMediaPostType(subPostDto);
        //        var subPost = mapper.Map<Domain.Entities.Post>(subPostDto);
        //        subPost.Audience = null;
        //        subPost.OwnerId = currentProfileId;
        //        subPosts.Add(subPost);
        //    }
        //    post.SubPosts = subPosts;

        //    var response = await postService.Create(post);

        //    foreach (var subPost in response.SubPosts)
        //        await postService.ConfirmSave(
        //            subPost.Id.ToString(),
        //            post.OwnerId.ToString(),
        //            subPost.FileMetadataId.ToString());

        //    return Created(string.Empty, new
        //    {
        //        id = response.Id,
        //    });
        //}

        [EndpointDescription("Update a post")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePostRequest request)
        {
            var existingPost = await postService.GetById(id.ToString())
                    ?? throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);

            if (!IsOwner(existingPost.OwnerId.ToString()))
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

            var post = mapper.Map<Domain.Entities.Post>(request);
            post.Id = existingPost.Id;

            await postService.Update(post);

            return Ok(new CommonMessageResponse(
                localizer["Message.UpdatedPostSuccess", id].ToString()
            ));
        }

        [EndpointDescription("Delete a post")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await postService.SoftDelete(id);
            return Ok(new CommonMessageResponse(
                localizer["Message.DeletedPostSuccess", id].ToString()
            ));
        }

        [Authorize]
        [EndpointDescription("Get post actions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{id}/actions")]
        public async Task<IActionResult> GetProfileActions(string id)
        {
            logger.LogInformation("Retrieve user profile");

            var post = await postService.GetById(id)
                ?? throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);

            bool isMe = IsOwner(post.OwnerId.ToString());

            IDictionary<string, bool> actions = new Dictionary<string, bool>
            {
                { PostActions.PostEdit.ToString(), isMe },
                { PostActions.PostDelete.ToString(), isMe }
            };


            return Ok(actions);
        }

        [EndpointDescription("Get profile post")]
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostById(string id)
        {
            Guid currentProfileId = GetCurrentProfileId();

            var whoCantSee = await postService.WhoCantSee(id, currentProfileId.ToString());

            if (whoCantSee.Contains(currentProfileId.ToString()))
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

            var result = await postService.GetById(id);

            var response = await postService.ToResponse(result, currentProfileId, mapper);

            return Ok(response);
        }

        [EndpointDescription("Get profile post")]
        [Authorize]
        [HttpGet("profiles/{profileId}")]
        [ProducesResponseType(typeof(CursorPagedResult<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimeline(string profileId, [FromQuery] string cursor = null, [FromQuery] int limit = 10)
        {
            Guid currentProfileId = GetCurrentProfileId();

            var result = await postService.GetProfilePost(currentProfileId.ToString(), profileId, cursor, limit);

            var response = new CursorPagedResult<BasePostResponse>
            {
                Items = await postService.ToResponse(result.Items, currentProfileId, mapper),
                NextCursor = result.NextCursor
            };

            return Ok(response);
        }

        [EndpointDescription("Get news feed")]
        [Authorize]
        [HttpGet("timeline")]
        [ProducesResponseType(typeof(CursorPagedResult<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimeline([FromQuery] string cursor = null, [FromQuery] int limit = 10)
        {
            Guid currentProfileId = GetCurrentProfileId();

            var result = await postService.GetTimeline(currentProfileId.ToString(), cursor, limit);

            var response = new CursorPagedResult<BasePostResponse>
            {
                Items = await postService.ToResponse(result.Items, currentProfileId, mapper),
                NextCursor = result.NextCursor
            };

            return Ok(response);
        }

        [EndpointDescription("Get news feed")]
        [Authorize]
        [HttpGet("search")]
        [ProducesResponseType(typeof(CursorPagedResult<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string keywords, [FromQuery] string cursor = null, [FromQuery] int limit = 10)
        {
            Guid currentProfileId = GetCurrentProfileId();

            var result = await postService.Search(currentProfileId.ToString(), keywords, cursor, limit);

            var response = new CursorPagedResult<BasePostResponse>
            {
                Items = await postService.ToResponse(result.Items, currentProfileId, mapper),
                NextCursor = result.NextCursor
            };

            return Ok(response);
        }

        [HttpGet("group-post/{groupId}")]
        public async Task<IActionResult> GetPostsByGroupId(string groupId)
        {
            IEnumerable<Domain.Entities.Post> response = await postService.GetAllByGroupId(groupId);
            return Ok(PostHelper.ToResponses(response));
        }

    }
}
