using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Application.Helpers;
using InfinityNetServer.Services.Post.Application.IServices;
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
        CommonFileClient fileClient,
        CommonCommentClient commentClient,
        CommonReactionClient reactionClient,
        IMessageBus messageBus,
        IMapper mapper,
        ILogger<PostController> logger,
        IStringLocalizer<PostSharedResource> localizer) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Create a text post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        [HttpPost("text")]
        public async Task<IActionResult> CreateTextPost([FromBody] CreatePostBaseRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            postService.ValidateAudienceType(request.Audience);

            var post = mapper.Map<Domain.Entities.Post>(request);
            post.OwnerId = currentProfileId;

            logger.LogInformation("Create media post: {0}", post);

            var response = await postService.Create(post, messageBus);
            return Created(string.Empty, new
            {
                id = response.Id,
            });
        }

        [EndpointDescription("Create a share post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        [HttpPost("share")]
        public async Task<IActionResult> CreateSharePost([FromBody] CreateSharePostRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            postService.ValidateAudienceType(request.Audience);
            _ = await postService.GetById(request.ShareId)
                ?? throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);

            var post = mapper.Map<Domain.Entities.Post>(request);
            post.OwnerId = currentProfileId;

            var response = await postService.Create(post, messageBus);
            return Created(string.Empty, new
            {
                id = response.Id,
            });
        }

        [EndpointDescription("Create a media post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        [HttpPost("media")]
        public async Task<IActionResult> CreateMediaPost([FromBody] CreateMediaPostRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            postService.ValidateMediaPostType(request);

            postService.ValidateAudienceType(request.Audience);

            var post = mapper.Map<Domain.Entities.Post>(request);
            post.OwnerId = currentProfileId;

            logger.LogInformation("Create media post: {0}", post);

            var response = await postService.Create(post, messageBus);

            await postService.ConfirmSave(
                response.Id.ToString(),
                post.OwnerId.ToString(),
                post.FileMetadataId.ToString(), messageBus);

            return Created(string.Empty, new
            {
                id = response.Id,
            });
        }

        [EndpointDescription("Create a multi-media post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        [HttpPost("multi-media")]
        public async Task<IActionResult> CreateMultiMediaPost([FromBody] CreateMultiMediaPostRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            postService.ValidateAudienceType(request.Audience);

            var post = mapper.Map<Domain.Entities.Post>(request);
            post.OwnerId = currentProfileId;

            var subPosts = new List<Domain.Entities.Post>();
            foreach (var subPostDto in request.Aggregates)
            {
                postService.ValidateMediaPostType(subPostDto);
                var subPost = mapper.Map<Domain.Entities.Post>(subPostDto);
                subPost.Audience = null;
                subPost.OwnerId = currentProfileId;
                subPosts.Add(subPost);
            }
            post.SubPosts = subPosts;

            var response = await postService.Create(post, messageBus);

            foreach (var subPost in response.SubPosts)
                await postService.ConfirmSave(
                    subPost.Id.ToString(),
                    post.OwnerId.ToString(),
                    subPost.FileMetadataId.ToString(), messageBus);

            return Created(string.Empty, new
            {
                id = response.Id,
            });
        }

        [EndpointDescription("Update a post")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePostRequest request)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            Guid.TryParse(id, out var postId);

            var post = mapper.Map<Domain.Entities.Post>(request);
            post.Id = postId;

            await postService.Update(post, messageBus);
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

        [EndpointDescription("Get profile post")]
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostById(string id)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var whoCantSee = await postService.WhoCantSee(id, currentProfileId.ToString());

            if (whoCantSee.Contains(currentProfileId.ToString()))
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

            var result = await postService.GetById(id);

            var response = await postService.ToResponse(result, currentProfileId, commentClient, reactionClient, fileClient, mapper);

            return Ok(response);
        }

        [EndpointDescription("Get profile post")]
        [Authorize]
        [HttpGet("profiles/{profileId}")]
        [ProducesResponseType(typeof(CursorPagedResult<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimeline(string profileId, [FromQuery] string cursor = null, [FromQuery] int limit = 10)
        {
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var result = await postService.GetProfilePost(currentProfileId.ToString(), profileId, cursor, limit);

            var response = new CursorPagedResult<BasePostResponse>
            {
                Items = await postService.ToResponse(result.Items, currentProfileId, commentClient, reactionClient, fileClient, mapper),
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
            Guid currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var result = await postService.GetTimeline(currentProfileId.ToString(), cursor, limit);

            var response = new CursorPagedResult<BasePostResponse>
            {
                Items = await postService.ToResponse(result.Items, currentProfileId, commentClient, reactionClient, fileClient, mapper),
                NextCursor = result.NextCursor
            };

            return Ok(response);

            // Popular comments and Reactions
            //IList<string> postIds = result.Items.Select(item => item.Id.ToString()).Distinct().ToList();

            //var popularCommentsTasks = postIds.Select(async id =>
            //{
            //    var comments = await commentClient.GetPopularComments(id.ToString());
            //    return comments;
            //});

            //var popularCommentsDict = (await Task.WhenAll(popularCommentsTasks))
            //    .Where(comments => comments.Count > 0).ToDictionary(x => x[0].PostId);

            //IList<(string postId, string profileId)> postIdsAndProfileIds = postIds.Select(p => (p, currentProfileId.ToString())).ToList();
            //var postReactionTypes = await reactionClient.GetPostReactionsByProfileIds(postIdsAndProfileIds);
            //var postReactionTypesDict = postReactionTypes.ToDictionary(
            //    previewReaction  => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            //var postReactionCounts = await reactionClient.GetPostReactionsCount(postIds);
            //var postReactionCountsDict = postReactionCounts.ToDictionary(
            //    reactionCount => Guid.Parse(reactionCount.postId), reactionCount => reactionCount.countDetails);

            //IList<string> commentIds = popularCommentsDict.Values
            //    .SelectMany(comment => comment.Select(c => c.Id.ToString())).Distinct().ToList();

            //IList<(string commentId, string profileId)> commentIdsAndProfileIds = commentIds.Select(p => (p, currentProfileId.ToString())).ToList();
            //var commentReactionTypes = await reactionClient.GetCommentReactionByProfileId(commentIdsAndProfileIds);
            //var commentReactionTypesDict = commentReactionTypes.ToDictionary(
            //    previewReaction => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            //var commentReactionCounts = await reactionClient.GetCommentReactionsCount(commentIds);
            //var commentReactionCountsDict = commentReactionCounts.ToDictionary(
            //    reactionCount => Guid.Parse(reactionCount.commentId), reactionCount => reactionCount.countDetails);

            //// Profiles
            //var profileIds = result.Items
            //    .SelectMany(item => item.Content.TagFacets.Select(t => t.ProfileId))
            //    .Concat(result.Items.Select(item => item.OwnerId))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.MultiMedia)
            //        .SelectMany(item => item.SubPosts.Select(p => p.OwnerId)))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.Share)
            //        .Select(item => item.Parent.OwnerId))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
            //        .SelectMany(item => item.Parent.SubPosts.Select(p => p.OwnerId)))
            //    .Concat(popularCommentsDict.Values.SelectMany(comment => comment.Select(c => c.Owner.Id)).Distinct().ToList())
            //    .Concat([currentProfileId])
            //    .Distinct();

            //var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            //var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            //// File Metadatas
            //var photoMetadataIds = profiles
            //    .Where(profile => profile.Avatar != null)
            //        .Select(profile => profile.Avatar.Id)
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.Photo)
            //        .Select(item => item.FileMetadataId.Value))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.MultiMedia)
            //        .SelectMany(item => item.SubPosts
            //            .Where(p => p.Type == PostType.Photo)
            //            .Select(p => p.FileMetadataId.Value)))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Photo)
            //        .Select(item => item.Parent.FileMetadataId.Value))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
            //        .SelectMany(item => item.Parent.SubPosts
            //            .Where(p => p.Type == PostType.Photo)
            //            .Select(p => p.FileMetadataId.Value)))
            //    .Distinct();

            //var videoMetadataIds = result.Items
            //    .Where(item => item.Type == PostType.Video)
            //    .Select(item => item.FileMetadataId.Value)
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.MultiMedia)
            //        .SelectMany(item => item.SubPosts
            //            .Where(p => p.Type == PostType.Video)
            //            .Select(p => p.FileMetadataId.Value)))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Video)
            //        .Select(item => item.Parent.FileMetadataId.Value))
            //    .Concat(result.Items
            //        .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
            //        .SelectMany(item => item.Parent.SubPosts
            //            .Where(p => p.Type == PostType.Video)
            //            .Select(p => p.FileMetadataId.Value)))
            //    .Distinct();

            //// Nạp metadata files trước nếu cần
            //var photoMetadataTasks = photoMetadataIds.Select(async id =>
            //{
            //    var metadata = await fileClient.GetPhotoMetadata(id.ToString());
            //    return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            //});
            //var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(photo => photo.Id, photo => photo.Metadata);

            //var videoMetadataTasks = videoMetadataIds.Select(async id =>
            //{
            //    var metadata = await fileClient.GetVideoMetadata(id.ToString());
            //    return new { Id = id, Metadata = metadata ??= new VideoMetadataResponse { Id = id } };
            //});
            //var videoMetadataDict = (await Task.WhenAll(videoMetadataTasks)).ToDictionary(video => video.Id, video => video.Metadata);

            //// Xử lý response
            //CursorPagedResult<BasePostResponse> response = new()
            //{
            //    Items = result.Items.Select(postItem =>
            //    {
            //        logger.LogError("Post ID: {PostId}", postItem.Id.ToString());
            //        var postResponse = mapper.Map<BasePostResponse>(postItem);

            //        // Map TagFacets
            //        postResponse.Content.TagFacets = postResponse.Content.TagFacets.Select(tagFacet =>
            //        {
            //            if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile)) 
            //            {
            //                tagFacet.Profile.Id = profile.Id;
            //                tagFacet.Profile.Type = profile.Type;
            //            }
            //            return tagFacet;
            //        }).ToList();

            //        // Process Owner
            //        if (profileDict.TryGetValue(postItem.OwnerId, out var ownerProfile))
            //        {
            //            var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
            //            ownerProfile.Avatar = avatar;
            //            postResponse.Owner = ownerProfile;
            //        }

            //        if (postReactionCountsDict.TryGetValue(postItem.Id, out var reactionCounts))
            //            postResponse.ReactionCounts = reactionCounts;

            //        if (postReactionTypesDict.TryGetValue(
            //            new { OwnerId = postItem.Id, ProfileId = currentProfileId }, out var reactionType))
            //            postResponse.Reaction = reactionType;

            //        if (popularCommentsDict.TryGetValue(postItem.Id, out var comments))
            //            postResponse.PopularComments = comments.Select(comment =>
            //            {
            //                if (profileDict.TryGetValue(comment.Owner.Id, out var profile))
            //                {
            //                    var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
            //                    profile.Avatar = avatar;
            //                    comment.Owner = profile;
            //                }

            //                if (commentReactionTypesDict.TryGetValue(
            //                    new { OwnerId = comment.Id, ProfileId = currentProfileId }, out var reactionType))
            //                    comment.Reaction = reactionType;

            //                if (commentReactionCountsDict.TryGetValue(comment.Id, out var reactionCounts))
            //                    comment.ReactionCounts = reactionCounts;

            //                return comment;
            //            }).ToList();

            //        // Process Post Types
            //        switch (postItem.Type)
            //        {
            //            case PostType.Photo:
            //                if (postItem.FileMetadataId.HasValue)
            //                    postResponse.Photo = photoMetadataDict.GetValueOrDefault(postItem.FileMetadataId.Value);
            //                break;

            //            case PostType.Video:
            //                if (postItem.FileMetadataId.HasValue)
            //                    postResponse.Video = videoMetadataDict.GetValueOrDefault(postItem.FileMetadataId.Value);
            //                break;

            //            case PostType.MultiMedia:
            //                postResponse.Aggregates = postItem.SubPosts.Select(subPost =>
            //                {
            //                    var subPostResponse = mapper.Map<BasePostResponse>(subPost);

            //                    // Map TagFacets
            //                    subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
            //                    {
            //                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
            //                        {
            //                            tagFacet.Profile.Id = profile.Id;
            //                            tagFacet.Profile.Type = profile.Type;
            //                        }
            //                        return tagFacet;
            //                    }).ToList();

            //                    // Process SubPost Owner
            //                    if (profileDict.TryGetValue(subPost.OwnerId, out var subOwnerProfile))
            //                    {
            //                        var avatar = photoMetadataDict.GetValueOrDefault(subOwnerProfile.Avatar.Id);
            //                        subOwnerProfile.Avatar = avatar;
            //                        subPostResponse.Owner = subOwnerProfile;
            //                    }

            //                    // Handle SubPost Types
            //                    switch (subPost.Type)
            //                    {
            //                        case PostType.Photo:
            //                            if (subPost.FileMetadataId.HasValue)
            //                                subPostResponse.Photo = photoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
            //                            break;

            //                        case PostType.Video:
            //                            if (subPost.FileMetadataId.HasValue)
            //                                subPostResponse.Video = videoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
            //                            break;
            //                    }
            //                    subPostResponse.Audience = postResponse.Audience;
            //                    return subPostResponse;
            //                }).ToList();
            //                break;

            //            case PostType.Share:
            //                if (postItem.Parent != null)
            //                {
            //                    var parentResponse = mapper.Map<BasePostResponse>(postItem.Parent);

            //                    // Process Parent's TagFacets
            //                    parentResponse.Content.TagFacets = parentResponse.Content.TagFacets.Select(tagFacet =>
            //                    {
            //                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
            //                        {
            //                            tagFacet.Profile.Id = profile.Id;
            //                            tagFacet.Profile.Type = profile.Type;
            //                        }
            //                        return tagFacet;
            //                    }).ToList();

            //                    // Process Parent's Owner
            //                    if (profileDict.TryGetValue(postItem.Parent.OwnerId, out var parentOwnerProfile))
            //                    {
            //                        var avatar = photoMetadataDict.GetValueOrDefault(parentOwnerProfile.Avatar.Id);
            //                        parentOwnerProfile.Avatar = avatar;
            //                        parentResponse.Owner = parentOwnerProfile;
            //                    }

            //                    // Handle Parent's Post Type
            //                    switch (postItem.Parent.Type)
            //                    {
            //                        case PostType.Photo:
            //                            if (postItem.Parent.FileMetadataId.HasValue)
            //                                parentResponse.Photo = photoMetadataDict.GetValueOrDefault(postItem.Parent.FileMetadataId.Value);
            //                            break;

            //                        case PostType.Video:
            //                            if (postItem.Parent.FileMetadataId.HasValue)
            //                                parentResponse.Video = videoMetadataDict.GetValueOrDefault(postItem.Parent.FileMetadataId.Value);
            //                            break;

            //                        case PostType.MultiMedia:
            //                            parentResponse.Aggregates = postItem.Parent.SubPosts.Select(subPost =>
            //                            {
            //                                var subPostResponse = mapper.Map<BasePostResponse>(subPost);

            //                                // Map TagFacets
            //                                subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
            //                                {
            //                                    if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
            //                                    {
            //                                        tagFacet.Profile.Id = profile.Id;
            //                                        tagFacet.Profile.Type = profile.Type;
            //                                    }
            //                                    return tagFacet;
            //                                }).ToList();

            //                                // Process SubPost Owner
            //                                if (profileDict.TryGetValue(subPost.OwnerId, out var subOwnerProfile))
            //                                {
            //                                    var avatar = photoMetadataDict.GetValueOrDefault(subOwnerProfile.Avatar.Id);
            //                                    subOwnerProfile.Avatar = avatar;
            //                                    subPostResponse.Owner = subOwnerProfile;
            //                                }

            //                                // Handle SubPost Types
            //                                switch (subPost.Type)
            //                                {
            //                                    case PostType.Photo:
            //                                        if (subPost.FileMetadataId.HasValue)
            //                                            subPostResponse.Photo = photoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
            //                                        break;

            //                                    case PostType.Video:
            //                                        if (subPost.FileMetadataId.HasValue)
            //                                            subPostResponse.Video = videoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
            //                                        break;
            //                                }
            //                                subPostResponse.Audience = parentResponse.Audience;
            //                                return subPostResponse;
            //                            }).ToList();
            //                            break;
            //                    }

            //                    postResponse.Share = parentResponse;
            //                }
            //                break;
            //        }

            //        return postResponse;
            //    }).ToList(),
            //    NextCursor = result.NextCursor
            //};

        }

        [HttpGet("group-post/{groupId}")]
        public async Task<IActionResult> GetPostsByGroupId(string groupId)
        {
            IEnumerable<Domain.Entities.Post> response = await postService.GetAllByGroupId(groupId);
            return Ok(PostHelper.ToResponses(response));
        }

    }
}
