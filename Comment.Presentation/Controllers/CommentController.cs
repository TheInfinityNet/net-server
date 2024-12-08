using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Comment.Application;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using InfinityNetServer.Services.Comment.Application.IServices;
using InfinityNetServer.Services.Comment.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        CommonProfileClient profileClient,
        CommonFileClient fileClient,
        CommonReactionClient reactionClient) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Get comments of post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("/posts/{postId}")]
        public async Task<IActionResult> GetByPostId(
            string postId, 
            [FromQuery] string cursor = null, 
            [FromQuery] int limit = 10, 
            [FromQuery] bool newest = true)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var result = await commentService.GetByPostId
                (postId, cursor, limit, newest ? SortDirection.Descending : SortDirection.Ascending);

            IList<string> commentIds = result.Items.Select(item => item.Id.ToString()).Distinct().ToList();

            IList<(string commentId, string profileId)> commentIdsAndProfileIds = 
                commentIds.Select(p => (p, currentProfileId.ToString())).ToList();
            var commentReactionTypes = await reactionClient.GetCommentReactionByProfileId(commentIdsAndProfileIds);
            var commentReactionTypesDict = commentReactionTypes.ToDictionary(
                previewReaction => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            var commentReactionCounts = await reactionClient.GetCommentReactionsCount(commentIds);
            var commentReactionCountsDict = commentReactionCounts.ToDictionary(
                reactionCount => Guid.Parse(reactionCount.commentId), reactionCount => reactionCount.countDetails);

            // Profiles
            var profileIds = result.Items
                .SelectMany(item => item.Content.TagFacets.Select(t => t.ProfileId))
                .Concat(result.Items.Select(item => item.ProfileId))
                .Concat([currentProfileId])
                .Distinct();

            var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            // File Metadatas
            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Concat(result.Items
                    .Where(item => item.Type == CommentType.Photo)
                    .Select(item => item.FileMetadataId.Value))
                .Distinct();

            var videoMetadataIds = result.Items
                .Where(item => item.Type == CommentType.Video)
                    .Select(item => item.FileMetadataId.Value)
                .Distinct();

            // Nạp metadata files trước nếu cần
            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(photo => photo.Id, photo => photo.Metadata);

            var videoMetadataTasks = videoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetVideoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new VideoMetadataResponse { Id = id } };
            });
            var videoMetadataDict = (await Task.WhenAll(videoMetadataTasks)).ToDictionary(video => video.Id, video => video.Metadata);

            CursorPagedResult<CommentResponse> response = new()
            {
                Items = result.Items.Select(commentItem =>
                {
                    logger.LogError("Post ID: {PostId}", commentItem.Id.ToString());
                    var commentResponse = mapper.Map<CommentResponse>(commentItem);

                    // Map TagFacets
                    commentResponse.Content.TagFacets = commentResponse.Content.TagFacets.Select(tagFacet =>
                    {
                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                        {
                            tagFacet.Profile.Id = profile.Id;
                            tagFacet.Profile.Type = profile.Type;
                        }
                        return tagFacet;
                    }).ToList();

                    // Process Owner
                    if (profileDict.TryGetValue(commentItem.ProfileId, out var ownerProfile))
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                        ownerProfile.Avatar = avatar;
                        commentResponse.Owner = ownerProfile;
                    }

                    if (commentReactionCountsDict.TryGetValue(commentItem.Id, out var reactionCounts))
                        commentResponse.ReactionCounts = reactionCounts;

                    if (commentReactionTypesDict.TryGetValue(
                        new { OwnerId = commentItem.Id, ProfileId = currentProfileId }, out var reactionType))
                        commentResponse.Reaction = reactionType;
                    return commentResponse;
                }).ToList(),

                NextCursor = result.NextCursor
            };

            return Ok(response);
        }

        [EndpointDescription("Get replied comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{parentId}/replies")]
        public async Task<IActionResult> GetReplies(string parentId, [FromQuery] string cursor = null, [FromQuery] int limit = 10)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var result = await commentService.GetReplies(parentId, cursor, limit);

            IList<string> commentIds = result.Items.Select(item => item.Id.ToString()).Distinct().ToList();

            IList<(string commentId, string profileId)> commentIdsAndProfileIds =
                commentIds.Select(p => (p, currentProfileId.ToString())).ToList();
            var commentReactionTypes = await reactionClient.GetCommentReactionByProfileId(commentIdsAndProfileIds);
            var commentReactionTypesDict = commentReactionTypes.ToDictionary(
                previewReaction => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            var commentReactionCounts = await reactionClient.GetCommentReactionsCount(commentIds);
            var commentReactionCountsDict = commentReactionCounts.ToDictionary(
                reactionCount => Guid.Parse(reactionCount.commentId), reactionCount => reactionCount.countDetails);

            // Profiles
            var profileIds = result.Items
                .SelectMany(item => item.Content.TagFacets.Select(t => t.ProfileId))
                .Concat(result.Items.Select(item => item.ProfileId))
                .Concat([currentProfileId])
                .Distinct();

            var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            // File Metadatas
            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Concat(result.Items
                    .Where(item => item.Type == CommentType.Photo)
                    .Select(item => item.FileMetadataId.Value))
                .Distinct();

            var videoMetadataIds = result.Items
                .Where(item => item.Type == CommentType.Video)
                    .Select(item => item.FileMetadataId.Value)
                .Distinct();

            // Nạp metadata files trước nếu cần
            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(photo => photo.Id, photo => photo.Metadata);

            var videoMetadataTasks = videoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetVideoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new VideoMetadataResponse { Id = id } };
            });
            var videoMetadataDict = (await Task.WhenAll(videoMetadataTasks)).ToDictionary(video => video.Id, video => video.Metadata);

            CursorPagedResult<CommentResponse> response = new()
            {
                Items = result.Items.Select(commentItem =>
                {
                    logger.LogError("Post ID: {PostId}", commentItem.Id.ToString());
                    var commentResponse = mapper.Map<CommentResponse>(commentItem);

                    // Map TagFacets
                    commentResponse.Content.TagFacets = commentResponse.Content.TagFacets.Select(tagFacet =>
                    {
                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                        {
                            tagFacet.Profile.Id = profile.Id;
                            tagFacet.Profile.Type = profile.Type;
                        }
                        return tagFacet;
                    }).ToList();

                    // Process Owner
                    if (profileDict.TryGetValue(commentItem.ProfileId, out var ownerProfile))
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                        ownerProfile.Avatar = avatar;
                        commentResponse.Owner = ownerProfile;
                    }

                    if (commentReactionCountsDict.TryGetValue(commentItem.Id, out var reactionCounts))
                        commentResponse.ReactionCounts = reactionCounts;

                    if (commentReactionTypesDict.TryGetValue(
                        new { OwnerId = commentItem.Id, ProfileId = currentProfileId }, out var reactionType))
                        commentResponse.Reaction = reactionType;
                    return commentResponse;
                }).ToList(),

                NextCursor = result.NextCursor
            };

            return Ok(response);
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

            commentService.ValidateType(comment);

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

            Domain.Entities.Comment comment = mapper.Map<Domain.Entities.Comment>(request);
            comment.Id = Guid.Parse(id);
            comment.ProfileId = currentProfileId;

            commentService.ValidateType(comment);

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
