using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Application.IServices;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public class ProfileFollowService(
    IProfileFollowRepository profileFollowRepository,
    IMessageBus messageBus,
    ILogger<ProfileFollowService> logger,
    IStringLocalizer<RelationshipSharedResource> localizer) : IProfileFollowService
    {

        public async Task<bool> HasFollowed(string currentProfileId, string targetProfileId)
            => await profileFollowRepository.GetByFollowerIdAndFolloweeIdAsync(Guid.Parse(currentProfileId), Guid.Parse(targetProfileId)) != null;

        public async Task<IList<string>> GetAllFolloweeIds(string currentProfileId)
        {
            var followeeIds = await profileFollowRepository.GetAllFolloweeIdsAsync(Guid.Parse(currentProfileId));
            return followeeIds.Select(i => i.ToString()).ToList();
        }

        public async Task<IList<string>> GetAllFollowerIds(string currentProfileId)
        {
            var followerIds = await profileFollowRepository.GetAllFollowerIdsAsync(Guid.Parse(currentProfileId));
            return followerIds.Select(i => i.ToString()).ToList();
        }

        public async Task<ProfileFollow> GetByFollowerIdAndFolloweeIdAsync(string followerId, string followeeId)
            => await profileFollowRepository.GetByFollowerIdAndFolloweeIdAsync(Guid.Parse(followerId), Guid.Parse(followeeId));

        public async Task<CursorPagedResult<ProfileFollow>> GetFollowers(string profileId, string cursor, int limit)
        {
            IList<string> followerIds = await GetAllFollowerIds(profileId);

            var specification = new SpecificationWithCursor<ProfileFollow>
            {
                Criteria = profileFollow => followerIds.Contains(profileFollow.FollowerId.ToString()),
                OrderFields = [
                        new OrderField<ProfileFollow>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                Limit = limit
            };
            return await profileFollowRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<ProfileFollow>> GetFollowees(string profileId, string cursor, int limit)
        {
            IList<string> followeeIds = await GetAllFolloweeIds(profileId);

            var specification = new SpecificationWithCursor<ProfileFollow>
            {
                Criteria = profileFollow => followeeIds.Contains(profileFollow.FolloweeId.ToString()),
                OrderFields = [
                        new OrderField<ProfileFollow>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                Limit = limit
            };
            return await profileFollowRepository.GetPagedAsync(specification);
        }

        public async Task<ProfileFollow> Follow(string followerId, string followeeId)
        { 
            var follow = await profileFollowRepository.CreateAsync(new ProfileFollow
               {
                   FollowerId = Guid.Parse(followerId),
                   FolloweeId = Guid.Parse(followeeId)
               });
            await PublishProfileFollowNotificationCommands(follow);
            return follow;
        }

        public async Task<UnFollowResponse> UnFollow(string followId)
        {
            await profileFollowRepository.DeleteAsync(Guid.Parse(followId));
            return new UnFollowResponse
            {
                Message = "UnFollowed",
                Status = "UnFollowed",
                UserId = followId
            };
        }

        private async Task PublishProfileFollowNotificationCommands(ProfileFollow entity)
        {
            Guid id = entity.Id;
            Guid followerId = entity.FollowerId;
            Guid followeeId = entity.FolloweeId;
            DateTime createdAt = entity.CreatedAt;

            var notificationCommand = new DomainCommand.CreateProfileFollowNotificationCommand
            {
                TriggeredBy = followerId.ToString(),
                TargetProfileId = followeeId,
                ProfileFollowId = id,
                Type = BuildingBlocks.Domain.Enums.NotificationType.NewProfileFollower,
                CreatedAt = createdAt
            };

            await messageBus.Publish(notificationCommand);
        }

    }

}

