using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Reaction.Application;
using InfinityNetServer.Services.Reaction.Application.DTOs.Requests;
using InfinityNetServer.Services.Reaction.Application.DTOs.Responses;
using InfinityNetServer.Services.Reaction.Application.Exceptions;
using InfinityNetServer.Services.Reaction.Application.IServices;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Presentation.Controllers
{
    [Tags("Comment reacion APIs")]
    [ApiController]
    [Route("comments")]
    public class CommentReactionController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CommentReactionController> logger,
        IMapper mapper,
        IStringLocalizer<ReactionSharedResource> localizer,
        CommonProfileClient profileClient,
        CommonCommentClient commentClient,
        IMessageBus messageBus,
        CommonFileClient fileClient,
        ICommentReactionService service) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Get reactions of comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetByCommentId(
            string commentId,
            [FromQuery] string cursor = null,
            [FromQuery] int limit = 10,
            [FromQuery] string type = "Like")
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            ReactionType reactionType = Enum.Parse<ReactionType>(type);

            var result = await service.GetByCommentId(commentId, cursor, limit, reactionType);

            // Profiles
            var profileIds = result.Items
                .Select(item => item.ProfileId)
                .Concat([currentProfileId])
                .Distinct();

            var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            // File Metadatas
            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Distinct();

            // Nạp metadata files trước nếu cần
            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(photo => photo.Id, photo => photo.Metadata);

            CursorPagedResult<CommentReactionResponse> response = new()
            {
                Items = result.Items.Select(commentItem =>
                {
                    logger.LogError("Post ID: {PostId}", commentItem.Id.ToString());
                    var commentReactionResponse = mapper.Map<CommentReactionResponse>(commentItem);

                    // Process Owner
                    if (profileDict.TryGetValue(commentItem.ProfileId, out var ownerProfile))
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                        ownerProfile.Avatar = avatar;
                        commentReactionResponse.Profile = ownerProfile;
                    }

                    return commentReactionResponse;
                }).ToList(),

                NextCursor = result.NextCursor
            };

            return Ok(response);
        }

        [EndpointDescription("Create reaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("{commentId}")]
        public async Task<IActionResult> Save(string commentId, [FromBody] ReactionRequest request)
        {
            try
            {
                Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                    : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

                var reaction = await service.Save(new CommentReaction
                {
                    CommentId = Guid.Parse(commentId),
                    ProfileId = currentProfileId,
                    Type = Enum.Parse<ReactionType>(request.Reaction)
                }, commentClient, messageBus);

                var reactionCounts = await service.CountByCommentId([commentId]);

                return Ok(new
                {
                    request.Reaction,
                    ReactionCounts = reactionCounts[0].countDetails
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ReactionException(ReactionError.CREATE_REACTION_FAILED, StatusCodes.Status422UnprocessableEntity);
            }
        }

        [EndpointDescription("Delete reaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete(string commentId)
        {
            try
            {
                Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                    : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

                var reaction = await service.Delete(commentId, currentProfileId.ToString());

                var reactionCounts = await service.CountByCommentId([commentId]);

                return Ok(new
                {
                    ReactionCounts = reactionCounts[0].countDetails
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw new ReactionException(ReactionError.DELETE_REACTION_FAILED, StatusCodes.Status422UnprocessableEntity);
            }

        }

    }
}