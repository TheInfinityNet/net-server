using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Pipelines.Sockets.Unofficial.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace InfinityNetServer.Services.Post.Presentation.Services
{
    public class PostService(
            IPostRepository postRepository,
            IPostPrivacyRepository postPrivacyRepository,
            CommonProfileClient profileClient,
            CommonRelationshipClient relationshipClient,
            IStringLocalizer<PostSharedResource> localizer,
            ILogger<PostService> logger) : IPostService
    {
        public async Task<IList<string>> GetAllPresentationIds()
        {
            var ids = await postRepository.GetAllPresentationIdsAsync();
            return ids.Select(id => id.ToString()).ToList();
        }

        public async Task<Domain.Entities.Post> GetById(string id)
            => await postRepository.GetByIdAsync(Guid.Parse(id));
        

        public async Task<IList<Domain.Entities.Post>> GetByType(string type)
            => await postRepository.GetByTypeAsync(Enum.Parse<PostType>(type));

        public async Task<IList<string>> WhoCanSee(string id)
        {
            var post = await GetById(id);
            var postPrivacy = await postPrivacyRepository.GetByPostIdAsync(post.Id);
            IList<string> followerIds = await relationshipClient.GetFollowerIds(post.OwnerId.ToString());
            IList<string> friendIds = await relationshipClient.GetFriendIds(post.OwnerId.ToString());
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(post.OwnerId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(post.OwnerId.ToString());
            IList<string> includeIds = postPrivacy.PostPrivacyIncludes.Select(i => i.ProfileId.ToString()).ToList();
            IList<string> excludeIds = postPrivacy.PostPrivacyExcludes.Select(i => i.ProfileId.ToString()).ToList();

            IList<string> whoCanSee = [];

            switch (postPrivacy.Type)
            {
                case PostPrivacyType.Include:
                    whoCanSee = includeIds;
                    break;

                case PostPrivacyType.Exclude:
                    whoCanSee = friendIds.Except(excludeIds).ToList();
                    break;

                case PostPrivacyType.Custom:
                    whoCanSee = friendIds.Concat(includeIds).Except(excludeIds).ToList();
                    break;

                case PostPrivacyType.Friends:
                    whoCanSee = friendIds;
                    break;

                case PostPrivacyType.OnlyMe:
                    whoCanSee = [ post.OwnerId.ToString() ];
                    break; 
            }

            whoCanSee = whoCanSee.Except(blockerIds).Except(blockeeIds).Distinct().ToList();

            return whoCanSee;
        }

        public async Task<IList<string>> WhoCantSee(string id)
        {
            var post = await GetById(id);
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(post.OwnerId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(post.OwnerId.ToString());
            IList<string> excludeIds = post.Privacy.PostPrivacyExcludes.Select(i => i.ProfileId.ToString()).ToList();

            return excludeIds.Concat(blockerIds).Concat(blockeeIds).Distinct().ToList();
        }

        public async Task<CursorPagedResult<Domain.Entities.Post>> GetNewsFeed(string profileId, string cursor, int pageSize)
        {
            var profile = await profileClient.GetProfile(profileId);
            IList<string> followeeIds = await relationshipClient.GetFolloweeIds(profileId);
            IList<string> friendIds = await relationshipClient.GetFriendIds(profileId);
            IList<string> potentialProfileIds = await profileClient.GetPotentialProfileIds(profile.Location);
            potentialProfileIds.Add(profileId);

            var specification = new SpecificationWithCursor<Domain.Entities.Post>
            {
                Criteria = post => 
                            post.PresentationId == null 
                            && followeeIds.Concat(friendIds).Concat(potentialProfileIds).Take(200).Contains(post.OwnerId.ToString()) 
                            && post.Privacy.PostPrivacyExcludes.All(i => i.ProfileId.ToString() != profileId) == true,
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
