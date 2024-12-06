using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Post;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Application.IServices;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            IPostService postService,
            CommonProfileClient profileClient,
            CommonFileClient fileClient,
            CommonCommentClient commentClient,
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
                throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);
            }

            switch (post.Type)
            {
                case PostType.Photo:
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

                case PostType.Video:
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
                    throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status400BadRequest);
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

            // Tập hợp toàn bộ các ID cần nạp trước
            var profileIds = result.Items
                .SelectMany(item => item.Content.TagFacets.Select(t => t.ProfileId))
                .Concat(result.Items.Select(item => item.OwnerId))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts.Select(p => p.OwnerId)))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share)
                    .Select(item => item.Parent.OwnerId))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts
                        .Select(p => p.OwnerId)))
                .Distinct();

            // Nạp toàn bộ profiles cần thiết
            var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Photo)
                    .Select(item => item.FileMetadataId.Value))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts
                        .Where(p => p.Type == PostType.Photo)
                        .Select(p => p.FileMetadataId.Value)))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Photo)
                    .Select(item => item.Parent.FileMetadataId.Value))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts
                        .Where(p => p.Type == PostType.Photo)
                        .Select(p => p.FileMetadataId.Value)))
                .Distinct();

            var videoMetadataIds = result.Items
                .Where(item => item.Type == PostType.Video)
                .Select(item => item.FileMetadataId.Value)
                .Concat(result.Items
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts
                        .Where(p => p.Type == PostType.Video)
                        .Select(p => p.FileMetadataId.Value)))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Video)
                    .Select(item => item.Parent.FileMetadataId.Value))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts
                        .Where(p => p.Type == PostType.Video)
                        .Select(p => p.FileMetadataId.Value)))
                .Distinct();

            // Nạp metadata files trước nếu cần
            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            var videoMetadataTasks = videoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetVideoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new VideoMetadataResponse { Id = id } };
            });
            var videoMetadataDict = (await Task.WhenAll(videoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            // Xử lý response
            CursorPagedResult<BasePostResponse> response = new()
            {
                Items = await Task.WhenAll(result.Items.Select(async postItem =>
                {
                    var postResponse = mapper.Map<BasePostResponse>(postItem);

                    // Map TagFacets
                    postResponse.Content.TagFacets = postResponse.Content.TagFacets.Select(tagFacet =>
                    {
                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                            tagFacet.Profile = mapper.Map<PreviewProfileResponse>(profile);
                        tagFacet.Profile.Avatar = null;
                        return tagFacet;
                    }).ToList();

                    // Process Owner
                    if (profileDict.TryGetValue(postItem.OwnerId, out var ownerProfile))
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                        ownerProfile.Avatar = avatar;
                        postResponse.Owner = ownerProfile;
                    }

                    // Process Post Types
                    switch (postItem.Type)
                    {
                        case PostType.Photo:
                            if (postItem.FileMetadataId.HasValue)
                                postResponse.Photo = photoMetadataDict.GetValueOrDefault(postItem.FileMetadataId.Value);
                            break;

                        case PostType.Video:
                            if (postItem.FileMetadataId.HasValue)
                                postResponse.Video = videoMetadataDict.GetValueOrDefault(postItem.FileMetadataId.Value);
                            break;

                        case PostType.MultiMedia:
                            postResponse.Aggregates = await Task.WhenAll(postItem.SubPosts.Select(subPost =>
                            {
                                var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                                // Map TagFacets
                                subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
                                {
                                    if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                        tagFacet.Profile = mapper.Map<PreviewProfileResponse>(profile);
                                    tagFacet.Profile.Avatar = null;
                                    return tagFacet;
                                }).ToList();

                                // Process SubPost Owner
                                if (profileDict.TryGetValue(subPost.OwnerId, out var subOwnerProfile))
                                {
                                    var avatar = photoMetadataDict.GetValueOrDefault(subOwnerProfile.Avatar.Id);
                                    subOwnerProfile.Avatar = avatar;
                                    subPostResponse.Owner = subOwnerProfile;
                                }

                                // Handle SubPost Types
                                switch (subPost.Type)
                                {
                                    case PostType.Photo:
                                        if (subPost.FileMetadataId.HasValue)
                                            subPostResponse.Photo = photoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                        break;

                                    case PostType.Video:
                                        if (subPost.FileMetadataId.HasValue)
                                            subPostResponse.Video = videoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                        break;
                                }
                                subPostResponse.Audience = postResponse.Audience;
                                return Task.FromResult(subPostResponse);
                            }));
                            break;

                        case PostType.Share:
                            if (postItem.Parent != null)
                            {
                                var parentResponse = mapper.Map<BasePostResponse>(postItem.Parent);

                                // Process Parent's TagFacets
                                parentResponse.Content.TagFacets = parentResponse.Content.TagFacets.Select(tagFacet =>
                                {
                                    if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                        tagFacet.Profile = mapper.Map<PreviewProfileResponse>(profile);
                                    return tagFacet;
                                }).ToList();

                                // Process Parent's Owner
                                if (profileDict.TryGetValue(postItem.Parent.OwnerId, out var parentOwnerProfile))
                                {
                                    var avatar = photoMetadataDict.GetValueOrDefault(parentOwnerProfile.Avatar.Id);
                                    parentOwnerProfile.Avatar = avatar;
                                    parentResponse.Owner = parentOwnerProfile;
                                }

                                // Handle Parent's Post Type
                                switch (postItem.Parent.Type)
                                {
                                    case PostType.Photo:
                                        if (postItem.Parent.FileMetadataId.HasValue)
                                            parentResponse.Photo = photoMetadataDict.GetValueOrDefault(postItem.Parent.FileMetadataId.Value);
                                        break;

                                    case PostType.Video:
                                        if (postItem.Parent.FileMetadataId.HasValue)
                                            parentResponse.Video = videoMetadataDict.GetValueOrDefault(postItem.Parent.FileMetadataId.Value);
                                        break;

                                    case PostType.MultiMedia:
                                        parentResponse.Aggregates = await Task.WhenAll(postItem.Parent.SubPosts.Select(subPost =>
                                        {
                                            var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                                            // Process SubPost Owner
                                            if (profileDict.TryGetValue(subPost.OwnerId, out var subOwnerProfile))
                                            {
                                                var avatar = photoMetadataDict.GetValueOrDefault(subOwnerProfile.Avatar.Id);
                                                subOwnerProfile.Avatar = avatar;
                                                subPostResponse.Owner = subOwnerProfile;
                                            }

                                            // Handle SubPost Types
                                            switch (subPost.Type)
                                            {
                                                case PostType.Photo:
                                                    if (subPost.FileMetadataId.HasValue)
                                                        subPostResponse.Photo = photoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                                    break;

                                                case PostType.Video:
                                                    if (subPost.FileMetadataId.HasValue)
                                                        subPostResponse.Video = videoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                                    break;
                                            }
                                            subPostResponse.Audience = parentResponse.Audience;
                                            return Task.FromResult(subPostResponse);
                                        }));
                                        break;
                                }

                                postResponse.Share = parentResponse;
                            }
                            break;
                    }

                    return postResponse;
                })),
                NextCursor = result.NextCursor
            };


            return Ok(response);

        }

        [EndpointDescription("Get news feed")]
        [HttpGet("timeline")]
        [ProducesResponseType(typeof(CursorPagedResult<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTimeline([FromQuery] string cursor = null, [FromQuery] int pageSize = 10)
        {
            var result = await postService.GetNewsFeed("d2ae9499-cf46-4687-b560-7fe7aafa7d28", cursor, pageSize);

            // Tập hợp toàn bộ các ID cần nạp trước
            var profileIds = result.Items
                .SelectMany(item => item.Content.TagFacets.Select(t => t.ProfileId))
                .Concat(result.Items.Select(item => item.OwnerId))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts.Select(p => p.OwnerId)))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share)
                    .Select(item => item.Parent.OwnerId))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts
                        .Select(p => p.OwnerId)))
                .Distinct();

            // Nạp toàn bộ profiles cần thiết
            var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Photo)
                    .Select(item => item.FileMetadataId.Value))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts
                        .Where(p => p.Type == PostType.Photo)
                        .Select(p => p.FileMetadataId.Value)))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Photo)
                    .Select(item => item.Parent.FileMetadataId.Value))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts
                        .Where(p => p.Type == PostType.Photo)
                        .Select(p => p.FileMetadataId.Value)))
                .Distinct();

            var videoMetadataIds = result.Items
                .Where(item => item.Type == PostType.Video)
                .Select(item => item.FileMetadataId.Value)
                .Concat(result.Items
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts
                        .Where(p => p.Type == PostType.Video)
                        .Select(p => p.FileMetadataId.Value)))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Video)
                    .Select(item => item.Parent.FileMetadataId.Value))
                .Concat(result.Items
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts
                        .Where(p => p.Type == PostType.Video)
                        .Select(p => p.FileMetadataId.Value)))
                .Distinct();

            // Nạp metadata files trước nếu cần
            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            var videoMetadataTasks = videoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetVideoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new VideoMetadataResponse { Id = id } };
            });
            var videoMetadataDict = (await Task.WhenAll(videoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            // Xử lý response
            CursorPagedResult<BasePostResponse> response = new()
            {
                Items = await Task.WhenAll(result.Items.Select(async postItem =>
                {
                    logger.LogError("Post ID: {PostId}", postItem.Id.ToString());
                    var postResponse = mapper.Map<BasePostResponse>(postItem);

                    // Map TagFacets
                    postResponse.Content.TagFacets = postResponse.Content.TagFacets.Select(tagFacet =>
                    {
                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                            tagFacet.Profile = mapper.Map<PreviewProfileResponse>(profile);
                        tagFacet.Profile.Avatar = null;
                        return tagFacet;
                    }).ToList();

                    // Process Owner
                    if (profileDict.TryGetValue(postItem.OwnerId, out var ownerProfile))
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                        ownerProfile.Avatar = avatar;
                        postResponse.Owner = ownerProfile;
                    }

                    // Process Post Types
                    switch (postItem.Type)
                    {
                        case PostType.Photo:
                            if (postItem.FileMetadataId.HasValue)
                                postResponse.Photo = photoMetadataDict.GetValueOrDefault(postItem.FileMetadataId.Value);
                            break;

                        case PostType.Video:
                            if (postItem.FileMetadataId.HasValue)
                                postResponse.Video = videoMetadataDict.GetValueOrDefault(postItem.FileMetadataId.Value);
                            break;

                        case PostType.MultiMedia:
                            postResponse.Aggregates = await Task.WhenAll(postItem.SubPosts.Select(subPost =>
                            {
                                var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                                // Map TagFacets
                                subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
                                {
                                    if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                        tagFacet.Profile = mapper.Map<PreviewProfileResponse>(profile);
                                    tagFacet.Profile.Avatar = null;
                                    return tagFacet;
                                }).ToList();

                                // Process SubPost Owner
                                if (profileDict.TryGetValue(subPost.OwnerId, out var subOwnerProfile))
                                {
                                    var avatar = photoMetadataDict.GetValueOrDefault(subOwnerProfile.Avatar.Id);
                                    subOwnerProfile.Avatar = avatar;
                                    subPostResponse.Owner = subOwnerProfile;
                                }

                                // Handle SubPost Types
                                switch (subPost.Type)
                                {
                                    case PostType.Photo:
                                        if (subPost.FileMetadataId.HasValue)
                                            subPostResponse.Photo = photoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                        break;

                                    case PostType.Video:
                                        if (subPost.FileMetadataId.HasValue)
                                            subPostResponse.Video = videoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                        break;
                                }
                                subPostResponse.Audience = postResponse.Audience;
                                return Task.FromResult(subPostResponse);
                            }));
                            break;

                        case PostType.Share:
                            if (postItem.Parent != null)
                            {
                                var parentResponse = mapper.Map<BasePostResponse>(postItem.Parent);

                                // Process Parent's TagFacets
                                parentResponse.Content.TagFacets = parentResponse.Content.TagFacets.Select(tagFacet =>
                                {
                                    if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                        tagFacet.Profile = mapper.Map<PreviewProfileResponse>(profile);
                                    return tagFacet;
                                }).ToList();

                                // Process Parent's Owner
                                if (profileDict.TryGetValue(postItem.Parent.OwnerId, out var parentOwnerProfile))
                                {
                                    var avatar = photoMetadataDict.GetValueOrDefault(parentOwnerProfile.Avatar.Id);
                                    parentOwnerProfile.Avatar = avatar;
                                    parentResponse.Owner = parentOwnerProfile;
                                }

                                // Handle Parent's Post Type
                                switch (postItem.Parent.Type)
                                {
                                    case PostType.Photo:
                                        if (postItem.Parent.FileMetadataId.HasValue)
                                            parentResponse.Photo = photoMetadataDict.GetValueOrDefault(postItem.Parent.FileMetadataId.Value);
                                        break;

                                    case PostType.Video:
                                        if (postItem.Parent.FileMetadataId.HasValue)
                                            parentResponse.Video = videoMetadataDict.GetValueOrDefault(postItem.Parent.FileMetadataId.Value);
                                        break;

                                    case PostType.MultiMedia:
                                        parentResponse.Aggregates = await Task.WhenAll(postItem.Parent.SubPosts.Select(subPost =>
                                        {
                                            var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                                            // Process SubPost Owner
                                            if (profileDict.TryGetValue(subPost.OwnerId, out var subOwnerProfile))
                                            {
                                                var avatar = photoMetadataDict.GetValueOrDefault(subOwnerProfile.Avatar.Id);
                                                subOwnerProfile.Avatar = avatar;
                                                subPostResponse.Owner = subOwnerProfile;
                                            }

                                            // Handle SubPost Types
                                            switch (subPost.Type)
                                            {
                                                case PostType.Photo:
                                                    if (subPost.FileMetadataId.HasValue)
                                                        subPostResponse.Photo = photoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                                    break;

                                                case PostType.Video:
                                                    if (subPost.FileMetadataId.HasValue)
                                                        subPostResponse.Video = videoMetadataDict.GetValueOrDefault(subPost.FileMetadataId.Value);
                                                    break;
                                            }
                                            subPostResponse.Audience = parentResponse.Audience;
                                            return Task.FromResult(subPostResponse);
                                        }));
                                        break;
                                }

                                postResponse.Share = parentResponse;
                            }
                            break;
                    }

                    return postResponse;
                })),
                NextCursor = result.NextCursor
            };


            return Ok(response);

        }

        // post/sfhakjsfhaskjf
        [EndpointDescription("Get post by id")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentCountById(string id)
        {
            int commentCount = await commentClient.GetCommentCountByPostId(id);
            return Ok(commentCount);
        }
        /*
         2
         */

        [HttpPost("preview-comment/{postId}")]
        public async Task<IActionResult> GetTopCommentWithMostReplies(string postId)
        {
            var response = await commentClient.GetPopularComments(postId);

            if (response == null)
                return NotFound(new { message = "No comments found for the given post ID." });

            return Ok(response);
        }
    }

}
