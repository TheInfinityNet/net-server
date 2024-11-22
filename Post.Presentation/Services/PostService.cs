using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.Helpers;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.Localization;
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
            IStringLocalizer<PostSharedResource> localizer,
            ILogger<PostService> logger) : IPostService
    {

        public async Task<Domain.Entities.Post> CreatePost(CreatePostRequest request)
        {
            Domain.Entities.Post requestPost = PostHelper.FromCreateRequest(request);
            return await postRepository.CreateAsync(requestPost);
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

                            || (post.Audience.Type == PostAudienceType.OnlyMe && post.OwnerId.Equals(profileUuid))

                            || (post.Audience.Type == PostAudienceType.Include 
                                && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid)) 
                                && friendIds.Contains(post.OwnerId.ToString()))

                            || (post.Audience.Type == PostAudienceType.Exclude 
                                && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid)) 
                                && friendIds.Contains(post.OwnerId.ToString()))

                            || (post.Audience.Type == PostAudienceType.Custom 
                                && post.Audience.Includes.Any(i => i.ProfileId.Equals(profileUuid)) 
                                && !post.Audience.Excludes.Any(i => i.ProfileId.Equals(profileUuid))))

                        && (post.OwnerId.Equals(profileId) 
                            || friendIds.Contains(post.OwnerId.ToString()) || followeeIds.Contains(post.OwnerId.ToString()))

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

    }
}
