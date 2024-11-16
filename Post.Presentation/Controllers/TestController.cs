using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Post.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;

namespace InfinityNetServer.Services.Post.Presentation.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<TestController> logger,
            IStringLocalizer<PostSharedResource> Localizer,
            IPostService postService,
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
            file.SetTimeToLocal();
            return Ok(file);
        }

        [EndpointDescription("Get video metadata")]
        [HttpGet("get-video/{fileMetadataId}")]
        [ProducesResponseType(typeof(VideoMetadataResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVideo(string fileMetadataId)
        {
            VideoMetadataResponse file = await fileClient.GetVideoMetadata(fileMetadataId);
            file.SetTimeToLocal();
            file.Thumbnail.SetTimeToLocal();
            return Ok(file);
        }

    }
}
