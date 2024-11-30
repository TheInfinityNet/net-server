
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.Exceptions;
using InfinityNetServer.Services.Post.Application.Helpers;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Presentation.Services
{
    public class PostService(
            IPostRepository postRepository,
            CommonProfileClient profileClient,
            CommonRelationshipClient relationshipClient,
            ILogger<PostService> logger) : IPostService
    {

        public async Task<Domain.Entities.Post> Create(Domain.Entities.Post entity)
        {
            logger.LogInformation("Creating post");
            return await postRepository.CreateAsync(entity);
        }

        public async Task<Domain.Entities.Post> DeletePost(string id)
        {
            await postRepository.DeleteAsync(Guid.Parse(id));
            return null;
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

        public async Task<Domain.Entities.Post> UpdatePost(UpdatePostRequest request)
        {
            Domain.Entities.Post requestPost = PostHelper.FromUpdateRequest(request);
            await postRepository.UpdateAsync(requestPost);
            return requestPost;
        }

        public async Task<IList<Domain.Entities.Post>> GetByType(string type)
            => await postRepository.GetByTypeAsync(Enum.Parse<PostType>(type));

        public async Task<IList<string>> WhoCantSee(string id)
        {
            var post = await GetById(id);
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(post.OwnerId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(post.OwnerId.ToString());
            IList<string> excludeIds = post.Audience.Excludes.Select(i => i.ProfileId.ToString()).ToList();

            return excludeIds.Concat(blockerIds).Concat(blockeeIds).Distinct().ToList();
        }

        public async Task<CursorPagedResult<Domain.Entities.Post>> GetNewsFeed(string profileId, string cursor, int pageSize)
        {
            var profile = await profileClient.GetProfile(profileId);
            IList<string> followeeIds = await relationshipClient.GetFolloweeIds(profileId);
            IList<string> friendIds = await relationshipClient.GetFriendIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(profileId.ToString());

            Guid profileUuid = Guid.Parse(profileId);
            var specification = new SpecificationWithCursor<Domain.Entities.Post>
            {

                Criteria = post =>
                        post.Presentation == null

                        && (post.Audience.Type == PostAudienceType.Public

                            || post.OwnerId.Equals(profileId)

                            || (followeeIds.Contains(post.OwnerId.ToString()) && friendIds.Contains(post.OwnerId.ToString()))

                            || (post.Audience.Type == PostAudienceType.OnlyMe && post.OwnerId.Equals(profileUuid))

                            || (post.Audience.Type == PostAudienceType.Friends && friendIds.Contains(post.OwnerId.ToString()))

                            || (post.Audience.Type == PostAudienceType.Include
                                && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid))
                                && friendIds.Contains(post.OwnerId.ToString()))

                            || (post.Audience.Type == PostAudienceType.Exclude
                                && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid))
                                && friendIds.Contains(post.OwnerId.ToString()))

                            || (post.Audience.Type == PostAudienceType.Custom
                                && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid))
                                && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid))
                                && friendIds.Contains(post.OwnerId.ToString())
                            ))

                        && !blockerIds.Concat(blockeeIds).Contains(post.OwnerId.ToString()),

                OrderFields = [
                        new OrderField<Domain.Entities.Post>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = pageSize
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
                    if ((dto.Include == null && dto.Exclude == null)
                            || (dto.Include.Count == 0 && dto.Exclude.Count == 0))
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
                    await messageBus.Publish(new DomainEvent.PhotoMetadataEvent
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
                    await messageBus.Publish(new DomainEvent.VideoMetadataEvent
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

    }
}
