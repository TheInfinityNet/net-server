﻿using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
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
    [Tags("Post reacion APIs")]
    [ApiController]
    [Route("posts")]
    public class PostReactionController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<CommentReactionController> logger,
        IMapper mapper,
        IStringLocalizer<ReactionSharedResource> localizer,
        CommonProfileClient profileClient,
        CommonFileClient fileClient,
        IPostReactionService service) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Get reactions of comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetByPostId(
            string postId,
            [FromQuery] string cursor = null,
            [FromQuery] int limit = 10,
            [FromQuery] string type = "Like")
        {
            Guid currentProfileId = GetCurrentProfileId();

            ReactionType reactionType = Enum.Parse<ReactionType>(type);

            var result = await service.GetByPostId(postId, cursor, limit, reactionType);

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

            CursorPagedResult<PostReactionResponse> response = new()
            {
                Items = result.Items.Select(commentItem =>
                {
                    logger.LogError("Post ID: {PostId}", commentItem.Id.ToString());
                    var commentReactionResponse = mapper.Map<PostReactionResponse>(commentItem);

                    // Process Owner
                    if (profileDict.TryGetValue(commentItem.ProfileId, out var ownerProfile))
                    {
                        if (ownerProfile.Avatar != null)
                        {
                            var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                            ownerProfile.Avatar = avatar;
                        }
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
        [HttpPost("{postId}")]
        public async Task<IActionResult> Save(string postId, [FromBody] ReactionRequest request)
        {
            try
            {
                Guid currentProfileId = GetCurrentProfileId();

                var reaction = await service.Save(new PostReaction
                {
                    PostId = Guid.Parse(postId),
                    ProfileId = currentProfileId,
                    Type = Enum.Parse<ReactionType>(request.Reaction)
                });

                var reactionCounts = await service.CountByPostIdAsync([postId]);
                logger.LogInformation("Reaction counts: {ReactionCounts}", reactionCounts);
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
        [HttpDelete("{postId}")]
        public async Task<IActionResult> Delete(string postId)
        {
            try
            {
                Guid currentProfileId = GetCurrentProfileId();

                var reaction = await service.Delete(postId, currentProfileId.ToString());

                var reactionCounts = await service.CountByPostIdAsync([postId]);
                logger.LogInformation("Reaction counts: {ReactionCounts}", reactionCounts);
                return Ok(new
                {
                    ReactionCounts = reactionCounts.Count > 0 ? reactionCounts[0].countDetails : new Dictionary<ReactionType, int>()
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