using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using InfinityNetServer.Services.Post.Application.Exceptions;
using InfinityNetServer.Services.Post.Application.IServices;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Services
{
    public class PostService(
            IPostRepository postRepository,
            CommonProfileClient profileClient,
            CommonRelationshipClient relationshipClient,
            ILogger<PostService> logger) : IPostService
    {

        public async Task<Domain.Entities.Post> Create(Domain.Entities.Post entity, IMessageBus messageBus)
        {
            logger.LogInformation("Creating post");
            var post = await postRepository.CreateAsync(entity);
            await PublishPostNotificationCommands(post, messageBus);
            return post;
        }

        public async Task<Domain.Entities.Post> Update(Domain.Entities.Post entity, IMessageBus messageBus)
        {
            var existingPost = await GetById(entity.Id.ToString())
                ?? throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);

            existingPost.Content = entity.Content;
            existingPost.Audience = entity.Audience;
            logger.LogInformation("Updating post");
            var post = await postRepository.UpdateAsync(existingPost);
            logger.LogInformation("Updating post");
            await PublishPostNotificationCommands(post, messageBus);
            return post;
        }

        public async Task<Domain.Entities.Post> SoftDelete(string id)
        {
            logger.LogInformation("Soft deleting post");
            return await postRepository.SoftDeleteAsync(Guid.Parse(id));
        }

        public async Task<Domain.Entities.Post> Delete(string id)
        {
            logger.LogInformation("Deleting post");
            return await postRepository.DeleteAsync(Guid.Parse(id));
        }

        public async Task<IEnumerable<Domain.Entities.Post>> GetAll()
            => await postRepository.GetAllAsync();

        public async Task<IList<Domain.Entities.Post>> GetAllByPresentationId(string presentationId)
            => await postRepository.GetAllByPresentationIdAsync(Guid.Parse(presentationId));

        public async Task<IList<string>> GetAllPresentationIds()
        {
            var ids = await postRepository.GetAllPresentationIdsAsync();
            return ids.Select(id => id.ToString()).ToList();
        }

        public async Task<Domain.Entities.Post> GetById(string id)
            => await postRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<IList<Domain.Entities.Post>> GetAllByType(string type)
            => await postRepository.GetAllByTypeAsync(Enum.Parse<PostType>(type));

        public async Task<IList<string>> WhoCantSee(string id, string currentProfileId)
        {
            var post = await GetById(id);
            if (post.Audience.Type == PostAudienceType.OnlyMe) return [ currentProfileId ];

            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(post.OwnerId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(post.OwnerId.ToString());
            IList<string> excludeIds = post.Audience.Excludes.Select(i => i.ProfileId.ToString()).ToList();

            return excludeIds.Concat(blockerIds).Concat(blockeeIds).Distinct().ToList();
        }

        public async Task<CursorPagedResult<Domain.Entities.Post>> GetTimeline(string profileId, string cursor, int pageSize)
        {
            IList<string> followeeIds = await relationshipClient.GetAllFolloweeIds(profileId);
            IList<string> friendIds = await relationshipClient.GetAllFriendIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId);
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId);

            Guid profileUuid = Guid.Parse(profileId);
            var specification = new SpecificationWithCursor<Domain.Entities.Post>
            {

                Criteria = post =>
                        post.Presentation == null && post.GroupId == null && !post.IsDeleted

                        && (post.Audience.Type == PostAudienceType.Public

                            || post.OwnerId.Equals(profileId)

                            || followeeIds.Contains(post.OwnerId.ToString()) && friendIds.Contains(post.OwnerId.ToString())

                            || post.Audience.Type == PostAudienceType.Friends && friendIds.Contains(post.OwnerId.ToString())

                            || post.Audience.Type == PostAudienceType.Include
                                && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid))
                                && friendIds.Contains(post.OwnerId.ToString())

                            || post.Audience.Type == PostAudienceType.Exclude
                                && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid))
                                && friendIds.Contains(post.OwnerId.ToString())

                            || post.Audience.Type == PostAudienceType.Custom
                                && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid))
                                && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid))
                                && friendIds.Contains(post.OwnerId.ToString())

                            || post.Audience.Type == PostAudienceType.OnlyMe && post.OwnerId.Equals(profileUuid)
                            )

                        && !blockerIds.Concat(blockeeIds).Contains(post.OwnerId.ToString()),
                Cursor = cursor,
                Limit = pageSize
            };

            return await postRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<Domain.Entities.Post>> GetProfilePost(string currentProfileId, string profileId, string cursor, int pageSize)
        {
            bool isMyProfile = currentProfileId.Equals(profileId);
            IList<string> followeeIds = await relationshipClient.GetAllFolloweeIds(profileId);
            IList<string> friendIds = await relationshipClient.GetAllFriendIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId);
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId);

            Guid profileUuid = Guid.Parse(profileId);
            var specification = new SpecificationWithCursor<Domain.Entities.Post>
            {

                Criteria = post =>
                        post.Presentation == null && post.GroupId == null && !post.IsDeleted
                        && isMyProfile ? post.OwnerId.Equals(profileUuid) 

                        : post.OwnerId.Equals(profileUuid) 
                            && (post.Audience.Type.Equals(PostAudienceType.Public) 

                                && followeeIds.Contains(post.OwnerId.ToString()) && friendIds.Contains(post.OwnerId.ToString())

                                || post.Audience.Type.Equals(PostAudienceType.Friends) && friendIds.Contains(post.OwnerId.ToString())

                                || post.Audience.Type.Equals(PostAudienceType.Include)
                                    && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid))
                                    && friendIds.Contains(post.OwnerId.ToString())

                                || post.Audience.Type.Equals(PostAudienceType.Exclude)
                                    && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid))
                                    && friendIds.Contains(post.OwnerId.ToString())

                                || post.Audience.Type.Equals(PostAudienceType.Custom)
                                    && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid))
                                    && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid))
                                    && friendIds.Contains(post.OwnerId.ToString())
                                )
                            && !blockerIds.Concat(blockeeIds).Contains(post.OwnerId.ToString()),
                Cursor = cursor,
                Limit = pageSize
            };

            return await postRepository.GetPagedAsync(specification);
        }

        public async Task<IList<Domain.Entities.Post>> GetAllByParentId(string id)
        {
            return await postRepository.GetAllByParentIdAsync(Guid.Parse(id));
        }

        public async Task<IList<Domain.Entities.Post>> GetAllByGroupId(string id)
        {
            return await postRepository.GetAllByGroupIdAsync(Guid.Parse(id));
        }
        public async Task<IList<Domain.Entities.Post>> GetAllByOwnerId(string id)
        {
            return await postRepository.GetAllByOwnerIdAsync(Guid.Parse(id));
        }

        public void ValidateMediaPostType(CreateMediaPostRequest dto)
        {
            PostType type = Enum.Parse<PostType>(dto.Type);
            switch (type)
            {
                case PostType.Photo:
                    if (string.IsNullOrEmpty(dto.PhotoId))
                        throw new PostException(PostError.REQUIRED_PHOTO_ID, StatusCodes.Status400BadRequest);
                    break;

                case PostType.Video:
                    if (string.IsNullOrEmpty(dto.VideoId))
                        throw new PostException(PostError.REQUIRED_VIDEO_ID, StatusCodes.Status400BadRequest);
                    break;

                default:
                    throw new PostException(PostError.INVALID_POST_TYPE, StatusCodes.Status400BadRequest);
            }
        }

        public void ValidateAudienceType(BasePostAudience dto)
        {
            switch (dto.Type)
            {
                case nameof(PostAudienceType.Public)
                        or nameof(PostAudienceType.OnlyMe)
                        or nameof(PostAudienceType.Friends):
                    if (dto.Include != null || dto.Exclude != null)
                        throw new PostException(PostError.REQUIRED_INCLUDE, StatusCodes.Status400BadRequest);
                    break;

                case nameof(PostAudienceType.Include):
                    if (dto.Include == null || dto.Include.Count == 0)
                        throw new PostException(PostError.REQUIRED_INCLUDE, StatusCodes.Status400BadRequest);
                    break;

                case nameof(PostAudienceType.Exclude):
                    if (dto.Exclude == null || dto.Exclude.Count == 0)
                        throw new PostException(PostError.REQUIRED_EXCLUDE, StatusCodes.Status400BadRequest);
                    break;

                case nameof(PostAudienceType.Custom):
                    if (dto.Include == null && dto.Exclude == null
                            || dto.Include.Count == 0 && dto.Exclude.Count == 0)
                        throw new PostException(PostError.REQUIRED_INCLUDE_OR_EXCLUDE, StatusCodes.Status400BadRequest);
                    break;

                default:
                    throw new PostException(PostError.INVALID_AUDIENCE_TYPE, StatusCodes.Status400BadRequest);
            }
        }

        public async Task ConfirmSave(string id, string profileId, string fileMetadataId, IMessageBus messageBus)
        {
            Domain.Entities.Post post = await GetById(id)
                ?? throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);

            Guid fileMetadataGuid = post.FileMetadataId
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);

            switch (post.Type)
            {
                case PostType.Photo:
                    await messageBus.Publish(new DomainEvent.CreatePhotoMetadataEvent
                    {
                        FileMetadataId = fileMetadataGuid,
                        TempId = Guid.Parse(fileMetadataId),
                        OwnerId = Guid.Parse(id),
                        OwnerType = FileOwnerType.Post,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = Guid.Parse(profileId)
                    });
                    break;

                case PostType.Video:
                    await messageBus.Publish(new DomainEvent.CreateVideoMetadataEvent
                    {
                        FileMetadataId = fileMetadataGuid,
                        TempId = Guid.Parse(fileMetadataId),
                        OwnerId = Guid.Parse(id),
                        OwnerType = FileOwnerType.Post,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = Guid.Parse(profileId)
                    });
                    break;

                default:
                    throw new PostException(PostError.INVALID_POST_TYPE, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<IList<BasePostResponse>> ToResponse(
            IList<Domain.Entities.Post> posts, 
            Guid currentProfileId, 
            CommonCommentClient commentClient, 
            CommonReactionClient reactionClient, 
            CommonFileClient fileClient, IMapper mapper)
        {
            IList<string> postIds = posts.Select(item => item.Id.ToString()).Distinct().ToList();

            var popularCommentsTasks = postIds.Select(async id =>
            {
                var comments = await commentClient.GetPopularComments(id.ToString());
                return comments;
            });

            var popularCommentsDict = (await Task.WhenAll(popularCommentsTasks))
                .Where(comments => comments.Count > 0).ToDictionary(x => x[0].PostId);

            IList<(string postId, string profileId)> postIdsAndProfileIds = postIds.Select(p => (p, currentProfileId.ToString())).ToList();
            var postReactionTypes = await reactionClient.GetPostReactionsByProfileIds(postIdsAndProfileIds);
            var postReactionTypesDict = postReactionTypes.ToDictionary(
                previewReaction => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            var postReactionCounts = await reactionClient.GetPostReactionsCount(postIds);
            var postReactionCountsDict = postReactionCounts.ToDictionary(
                reactionCount => Guid.Parse(reactionCount.postId), reactionCount => reactionCount.countDetails);

            IList<string> commentIds = popularCommentsDict.Values
                .SelectMany(comment => comment.Select(c => c.Id.ToString())).Distinct().ToList();

            IList<(string commentId, string profileId)> commentIdsAndProfileIds = commentIds.Select(p => (p, currentProfileId.ToString())).ToList();
            var commentReactionTypes = await reactionClient.GetCommentReactionByProfileId(commentIdsAndProfileIds);
            var commentReactionTypesDict = commentReactionTypes.ToDictionary(
                previewReaction => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            var commentReactionCounts = await reactionClient.GetCommentReactionsCount(commentIds);
            var commentReactionCountsDict = commentReactionCounts.ToDictionary(
                reactionCount => Guid.Parse(reactionCount.commentId), reactionCount => reactionCount.countDetails);

            // Profiles
            var profileIds = posts
                .SelectMany(item => item.Content.TagFacets.Select(t => t.ProfileId))
                .Concat(posts.Select(item => item.OwnerId))
                .Concat(posts
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts.Select(p => p.OwnerId)))
            .Concat(posts
                    .Where(item => item.Type == PostType.Share)
                    .Select(item => item.Parent.OwnerId))
                .Concat(posts
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts.Select(p => p.OwnerId)))
                .Concat(popularCommentsDict.Values.SelectMany(comment => comment.Select(c => c.Owner.Id)).Distinct().ToList())
                .Concat([currentProfileId])
            .Distinct();

            var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            // File Metadatas
            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Concat(posts
                    .Where(item => item.Type == PostType.Photo)
                    .Select(item => item.FileMetadataId.Value))
                .Concat(posts
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts
                        .Where(p => p.Type == PostType.Photo)
                        .Select(p => p.FileMetadataId.Value)))
                .Concat(posts
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Photo)
                    .Select(item => item.Parent.FileMetadataId.Value))
                .Concat(posts
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.MultiMedia)
                    .SelectMany(item => item.Parent.SubPosts
                    .Where(p => p.Type == PostType.Photo)
                        .Select(p => p.FileMetadataId.Value)))
                .Distinct();

            var videoMetadataIds = posts
                .Where(item => item.Type == PostType.Video)
                .Select(item => item.FileMetadataId.Value)
                .Concat(posts
                    .Where(item => item.Type == PostType.MultiMedia)
                    .SelectMany(item => item.SubPosts
                        .Where(p => p.Type == PostType.Video)
                        .Select(p => p.FileMetadataId.Value)))
                .Concat(posts
                    .Where(item => item.Type == PostType.Share && item.Parent.Type == PostType.Video)
                    .Select(item => item.Parent.FileMetadataId.Value))
                .Concat(posts
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
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(photo => photo.Id, photo => photo.Metadata);

            var videoMetadataTasks = videoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetVideoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new VideoMetadataResponse { Id = id } };
            });
            var videoMetadataDict = (await Task.WhenAll(videoMetadataTasks)).ToDictionary(video => video.Id, video => video.Metadata);

            // Xử lý response
            return posts.Select(postItem =>
                {
                    logger.LogError("Post ID: {PostId}", postItem.Id.ToString());
                    var postResponse = mapper.Map<BasePostResponse>(postItem);

                    // Map TagFacets
                    postResponse.Content.TagFacets = postResponse.Content.TagFacets.Select(tagFacet =>
                    {
                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                        {
                            tagFacet.Profile.Id = profile.Id;
                            tagFacet.Profile.Type = profile.Type;
                        }
                        return tagFacet;
                    }).ToList();

                    // Process Owner
                    if (profileDict.TryGetValue(postItem.OwnerId, out var ownerProfile))
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                        ownerProfile.Avatar = avatar;
                        postResponse.Owner = ownerProfile;
                    }

                    if (postReactionCountsDict.TryGetValue(postItem.Id, out var reactionCounts))
                        postResponse.ReactionCounts = reactionCounts;

                    if (postReactionTypesDict.TryGetValue(
                        new { OwnerId = postItem.Id, ProfileId = currentProfileId }, out var reactionType))
                        postResponse.Reaction = reactionType;

                    if (popularCommentsDict.TryGetValue(postItem.Id, out var comments))
                        postResponse.PopularComments = comments.Select(comment =>
                        {
                            if (profileDict.TryGetValue(comment.Owner.Id, out var profile))
                            {
                                var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
                                profile.Avatar = avatar;
                                comment.Owner = profile;
                            }

                            if (commentReactionTypesDict.TryGetValue(
                                new { OwnerId = comment.Id, ProfileId = currentProfileId }, out var reactionType))
                                comment.Reaction = reactionType;

                            if (commentReactionCountsDict.TryGetValue(comment.Id, out var reactionCounts))
                                comment.ReactionCounts = reactionCounts;

                            return comment;
                        }).ToList();

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
                            postResponse.Aggregates = postItem.SubPosts.Select(subPost =>
                            {
                                var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                                // Map TagFacets
                                subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
                                {
                                    if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                    {
                                        tagFacet.Profile.Id = profile.Id;
                                        tagFacet.Profile.Type = profile.Type;
                                    }
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
                                return subPostResponse;
                            }).ToList();
                            break;

                        case PostType.Share:
                            if (postItem.Parent != null)
                            {
                                var parentResponse = mapper.Map<BasePostResponse>(postItem.Parent);

                                // Process Parent's TagFacets
                                parentResponse.Content.TagFacets = parentResponse.Content.TagFacets.Select(tagFacet =>
                                {
                                    if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                    {
                                        tagFacet.Profile.Id = profile.Id;
                                        tagFacet.Profile.Type = profile.Type;
                                    }
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
                                        parentResponse.Aggregates = postItem.Parent.SubPosts.Select(subPost =>
                                        {
                                            var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                                            // Map TagFacets
                                            subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
                                            {
                                                if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                                {
                                                    tagFacet.Profile.Id = profile.Id;
                                                    tagFacet.Profile.Type = profile.Type;
                                                }
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
                                            subPostResponse.Audience = parentResponse.Audience;
                                            return subPostResponse;
                                        }).ToList();
                                        break;
                                }

                                postResponse.Share = parentResponse;
                            }
                            break;
                    }

                    return postResponse;
                }).ToList();
        }

        public async Task<BasePostResponse> ToResponse(
            Domain.Entities.Post post,
            Guid currentProfileId,
            CommonCommentClient commentClient,
            CommonReactionClient reactionClient,
            CommonFileClient fileClient, IMapper mapper)
        {
            string postId = post.Id.ToString();

            var popularComments = await commentClient.GetPopularComments(postId);

            (string postId, string profileId) postIdAndProfileId = (postId, currentProfileId.ToString());
            var postReactionTypes = await reactionClient.GetPostReactionsByProfileIds([postIdAndProfileId]);
            var postReactionTypesDict = postReactionTypes.ToDictionary(
                previewReaction => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            var postReactionCounts = await reactionClient.GetPostReactionsCount([postId]);
            var postReactionCountsDict = postReactionCounts.ToDictionary(
                reactionCount => Guid.Parse(reactionCount.postId), reactionCount => reactionCount.countDetails);

            IList<string> commentIds = popularComments.Select(c => c.Id.ToString()).Distinct().ToList();

            IList<(string commentId, string profileId)> commentIdsAndProfileIds = commentIds.Select(p => (p, currentProfileId.ToString())).ToList();
            var commentReactionTypes = await reactionClient.GetCommentReactionByProfileId(commentIdsAndProfileIds);
            var commentReactionTypesDict = commentReactionTypes.ToDictionary(
                previewReaction => new { previewReaction.OwnerId, previewReaction.ProfileId }, previewReaction => previewReaction.Type);

            var commentReactionCounts = await reactionClient.GetCommentReactionsCount(commentIds);
            var commentReactionCountsDict = commentReactionCounts.ToDictionary(
                reactionCount => Guid.Parse(reactionCount.commentId), reactionCount => reactionCount.countDetails);

            // Profiles
            var profileIds = post.Content.TagFacets.Select(t => t.ProfileId)
                .Concat([post.OwnerId])
                .Concat(post.Type switch
                        {
                            PostType.MultiMedia => post.SubPosts.Select(p => p.OwnerId),
                            PostType.Share => post.Parent.Type switch
                            {
                                PostType.MultiMedia => post.Parent.SubPosts.Select(p => p.OwnerId),
                                _ => [post.Parent.OwnerId],
                            },
                            _ => []
                        }
                )
                .Concat(popularComments.Select(c => c.Owner.Id)).Distinct().ToList()
                .Concat([currentProfileId])
            .Distinct();

            var profiles = await profileClient.GetProfiles(profileIds.Select(id => id.ToString()).ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            // File Metadatas
            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Concat(post.Type switch
                {
                    PostType.Photo => [post.FileMetadataId.Value],
                    PostType.MultiMedia => post.SubPosts
                        .Where(p => p.Type == PostType.Photo).Select(p => p.FileMetadataId.Value),
                    PostType.Share => post.Parent.Type switch
                    {
                        PostType.Photo => [post.Parent.FileMetadataId.Value],
                        PostType.MultiMedia => post.Parent.SubPosts
                            .Where(p => p.Type == PostType.Photo).Select(p => p.FileMetadataId.Value),
                        _ => [],
                    },
                    _ => []
                }
                )
                .Distinct();

            var videoMetadataIds = (post.Type switch
            {
                PostType.Photo => [post.FileMetadataId.Value],
                PostType.MultiMedia => post.SubPosts
                    .Where(p => p.Type == PostType.Photo).Select(p => p.FileMetadataId.Value),
                PostType.Share => post.Parent.Type switch
                {
                    PostType.Photo => [post.Parent.FileMetadataId.Value],
                    PostType.MultiMedia => post.Parent.SubPosts
                        .Where(p => p.Type == PostType.Photo).Select(p => p.FileMetadataId.Value),
                    _ => [],
                },
                _ => []
            }).Distinct();

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

            logger.LogError("Post ID: {PostId}", post.Id.ToString());
            var postResponse = mapper.Map<BasePostResponse>(post);

            // Map TagFacets
            postResponse.Content.TagFacets = postResponse.Content.TagFacets.Select(tagFacet =>
            {
                if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                {
                    tagFacet.Profile.Id = profile.Id;
                    tagFacet.Profile.Type = profile.Type;
                }
                return tagFacet;
            }).ToList();

            // Process Owner
            if (profileDict.TryGetValue(post.OwnerId, out var ownerProfile))
            {
                var avatar = photoMetadataDict.GetValueOrDefault(ownerProfile.Avatar.Id);
                ownerProfile.Avatar = avatar;
                postResponse.Owner = ownerProfile;
            }

            if (postReactionCountsDict.TryGetValue(post.Id, out var reactionCounts))
                postResponse.ReactionCounts = reactionCounts;

            if (postReactionTypesDict.TryGetValue(
                new { OwnerId = post.Id, ProfileId = currentProfileId }, out var reactionType))
                postResponse.Reaction = reactionType;

            postResponse.PopularComments = popularComments.Select(comment =>
            {
                if (profileDict.TryGetValue(comment.Owner.Id, out var profile))
                {
                    var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
                    profile.Avatar = avatar;
                    comment.Owner = profile;
                }

                if (commentReactionTypesDict.TryGetValue(
                    new { OwnerId = comment.Id, ProfileId = currentProfileId }, out var reactionType))
                    comment.Reaction = reactionType;

                if (commentReactionCountsDict.TryGetValue(comment.Id, out var reactionCounts))
                    comment.ReactionCounts = reactionCounts;

                return comment;
            }).ToList();

            // Process Post Types
            switch (post.Type)
            {
                case PostType.Photo:
                    if (post.FileMetadataId.HasValue)
                        postResponse.Photo = photoMetadataDict.GetValueOrDefault(post.FileMetadataId.Value);
                    break;

                case PostType.Video:
                    if (post.FileMetadataId.HasValue)
                        postResponse.Video = videoMetadataDict.GetValueOrDefault(post.FileMetadataId.Value);
                    break;

                case PostType.MultiMedia:
                    postResponse.Aggregates = post.SubPosts.Select(subPost =>
                    {
                        var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                        // Map TagFacets
                        subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
                        {
                            if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                            {
                                tagFacet.Profile.Id = profile.Id;
                                tagFacet.Profile.Type = profile.Type;
                            }
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
                        return subPostResponse;
                    }).ToList();
                    break;

                case PostType.Share:
                    if (post.Parent != null)
                    {
                        var parentResponse = mapper.Map<BasePostResponse>(post.Parent);

                        // Process Parent's TagFacets
                        parentResponse.Content.TagFacets = parentResponse.Content.TagFacets.Select(tagFacet =>
                        {
                            if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                            {
                                tagFacet.Profile.Id = profile.Id;
                                tagFacet.Profile.Type = profile.Type;
                            }
                            return tagFacet;
                        }).ToList();

                        // Process Parent's Owner
                        if (profileDict.TryGetValue(post.Parent.OwnerId, out var parentOwnerProfile))
                        {
                            var avatar = photoMetadataDict.GetValueOrDefault(parentOwnerProfile.Avatar.Id);
                            parentOwnerProfile.Avatar = avatar;
                            parentResponse.Owner = parentOwnerProfile;
                        }

                        // Handle Parent's Post Type
                        switch (post.Parent.Type)
                        {
                            case PostType.Photo:
                                if (post.Parent.FileMetadataId.HasValue)
                                    parentResponse.Photo = photoMetadataDict.GetValueOrDefault(post.Parent.FileMetadataId.Value);
                                break;

                            case PostType.Video:
                                if (post.Parent.FileMetadataId.HasValue)
                                    parentResponse.Video = videoMetadataDict.GetValueOrDefault(post.Parent.FileMetadataId.Value);
                                break;

                            case PostType.MultiMedia:
                                parentResponse.Aggregates = post.Parent.SubPosts.Select(subPost =>
                                {
                                    var subPostResponse = mapper.Map<BasePostResponse>(subPost);

                                    // Map TagFacets
                                    subPostResponse.Content.TagFacets = subPostResponse.Content.TagFacets.Select(tagFacet =>
                                    {
                                        if (profileDict.TryGetValue(tagFacet.Profile.Id, out var profile))
                                        {
                                            tagFacet.Profile.Id = profile.Id;
                                            tagFacet.Profile.Type = profile.Type;
                                        }
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
                                    subPostResponse.Audience = parentResponse.Audience;
                                    return subPostResponse;
                                }).ToList();
                                break;
                        }

                        postResponse.Share = parentResponse;
                    }
                    break;
            }

            return postResponse;
        }

        private async Task PublishPostNotificationCommands(Domain.Entities.Post entity, IMessageBus messageBus)
        {
            Guid id = entity.Id;
            Guid ownerId = entity.OwnerId;
            DateTime createdAt = entity.CreatedAt;
            Domain.Entities.PostContent content = entity.Content;

            if (content.TagFacets.Count > 0)
            {
                foreach (var tag in content.TagFacets)
                {
                    Guid taggedProfileId = tag.ProfileId;

                    var notificationCommand = new DomainCommand.CreatePostNotificationCommand
                    {
                        Id = Guid.NewGuid(),
                        TriggeredBy = ownerId.ToString(),
                        TargetProfileId = taggedProfileId,
                        PostId = id,
                        Type = NotificationType.TaggedInPost,
                        CreatedAt = createdAt
                    };

                    await messageBus.Publish(notificationCommand);
                }
            }
        }

    }
}
