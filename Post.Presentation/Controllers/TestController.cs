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

namespace InfinityNetServer.Services.Post.Presentation.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<TestController> logger,
            IStringLocalizer<PostSharedResource> Localizer,
            IPostService postService,
            IMessageBus messageBus) : BaseApiController(authenticatedUserService)
    {

        [Authorize]
        [HttpPost("confirm-save")]
        public async Task<IActionResult> Test([FromBody] ConfirmSaveFileRequest request)
        {
            Guid fileMetadataId = Guid.Parse(request.FileMetadataId);
            try
            {
                var post = await postService.GetById(request.OwnerId);
                fileMetadataId = post.FileMetadataId ?? fileMetadataId;
            }
            catch
            {
                logger.LogError("Post not found");
            }

            await messageBus.Publish(new DomainEvent.PhotoMetadataEvent
            {
                Id = fileMetadataId,
                TempId = Guid.Parse(request.FileMetadataId),
                OwnerId = Guid.Parse(request.OwnerId),
                OwnerType = FileOwnerType.Post,
                UpdatedAt = DateTime.Now,
                UpdatedBy = GetCurrentUserId().Value
            });

            return Ok(new { Message = "Save file successfully" });
        }

    }
}
