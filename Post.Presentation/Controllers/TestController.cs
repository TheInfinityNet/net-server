using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Presentation.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<TestController> logger,
            IMapper mapper,
            IStringLocalizer<PostSharedResource> Localizer,
            IPostRepository postRepository,
            IPostService postService,
            CommonProfileClient profileClient,
            CommonFileClient fileClient,
            IMessageBus messageBus) : BaseApiController(authenticatedUserService)
    {

        [Authorize]
        [HttpPost("confirm-save")]
        public async Task<IActionResult> Test([FromBody] ConfirmSaveFileRequest request)
        {
            Domain.Entities.Post post;
            Guid fileMetadataId = Guid.Parse(request.FileMetadataId);
            try
            {
                post = await postService.GetById(request.OwnerId);
                fileMetadataId = post.FileMetadataId ?? fileMetadataId;
            }
            catch
            {
                logger.LogError("Post not found");
                throw new CommonException(BaseErrorCode.POST_NOT_FOUND, StatusCodes.Status404NotFound);
            }

            switch (post.Type)
            {
                case Domain.Enums.PostType.Photo:
                    await messageBus.Publish(new DomainEvent.PhotoMetadataEvent
                    {
                        Id = fileMetadataId,
                        TempId = Guid.Parse(request.FileMetadataId),
                        OwnerId = Guid.Parse(request.OwnerId),
                        OwnerType = FileOwnerType.Post,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = GetCurrentProfileId().Value
                    });
                    break;

                case Domain.Enums.PostType.Video:
                    await messageBus.Publish(new DomainEvent.VideoMetadataEvent
                    {
                        Id = fileMetadataId,
                        TempId = Guid.Parse(request.FileMetadataId),
                        OwnerId = Guid.Parse(request.OwnerId),
                        OwnerType = FileOwnerType.Post,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = GetCurrentProfileId().Value
                    });
                    break;
                default:
                    throw new CommonException(BaseErrorCode.POST_NOT_FOUND, StatusCodes.Status400BadRequest);
            }


            return Ok(new { Message = "Save file successfully" });
        }

        [EndpointDescription("Get photo metadata")]
        [HttpGet("get-photo/{fileMetadataId}")]
        [ProducesResponseType(typeof(PhotoMetadataResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPhoto(string fileMetadataId)
        {
            PhotoMetadataResponse file = await fileClient.GetPhotoMetadata(fileMetadataId);
            file ??= new VideoMetadataResponse();
            return Ok(file);
        }

        [EndpointDescription("Get video metadata")]
        [HttpGet("get-video/{fileMetadataId}")]
        [ProducesResponseType(typeof(VideoMetadataResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVideo(string fileMetadataId)
        {
            VideoMetadataResponse file = await fileClient.GetVideoMetadata(fileMetadataId);
            file ??= new VideoMetadataResponse();
            return Ok(file);
        }

        [EndpointDescription("Get news feed")]
        [Authorize]
        [HttpGet("news-feed")]
        [ProducesResponseType(typeof(CursorPagedResult<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNewsFeed([FromQuery] string cursor = null, [FromQuery] int pageSize = 10)
        {
            var profileId = GetCurrentProfileId().Value;
            var result = await postService.GetNewsFeed(profileId.ToString(), cursor, pageSize);

            CursorPagedResult<BasePostResponse> response = new()
            {
                Items = await Task.WhenAll(result.Items.Select(async item =>
                {
                    // Map the base response
                    var postResponse = mapper.Map<BasePostResponse>(item);

                    // Process each tag facet asynchronously
                    var tagFacets = postResponse.Content.TagFacets;
                    var updatedFacets = await Task.WhenAll(tagFacets.Select(async t =>
                    {
                        var profile = await profileClient.GetProfile(t.Profile.Id.ToString());
                        t.Profile = mapper.Map<PreviewProfileResponse>(profile);
                        return t; // Return the updated TagFacet
                    }));

                    // Update the TagFacets
                    postResponse.Content.TagFacets = [.. updatedFacets];

                    var owner = await profileClient.GetProfile(postResponse.Owner.Id.ToString());
                    var avatar = await fileClient.GetPhotoMetadata(owner.Avatar.Id.ToString());
                    var cover = await fileClient.GetPhotoMetadata(owner.Cover.Id.ToString());
                    postResponse.Owner = owner;

                    return postResponse;
                })),
                NextCursor = result.NextCursor
            };

            return Ok(response);

        }
    }
}
