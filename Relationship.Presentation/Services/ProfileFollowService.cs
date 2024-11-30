using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public class ProfileFollowService(
    IProfileFollowRepository profileFollowRepository,
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

        public async Task<ProfileFollow> Follow(string followerId, string followeeId)
        => await profileFollowRepository.CreateAsync(new ProfileFollow
        {
            FollowerId = Guid.Parse(followerId),
            FolloweeId = Guid.Parse(followeeId)
        });

        public async Task<UnFollowResponse> UnFollow(string followId)
        {
            await profileFollowRepository.DeleteAsync(Guid.Parse(followId));
            return new UnFollowResponse
            {
                Message = "UnFollowed",
                Status = "UnFollowed",
                UserId = Guid.Parse(followId)
            };
        }
        public async Task<ProfileFollow> GetByFollowerIdAndFolloweeIdAsync(string followerId, string followeeId)
            => await profileFollowRepository.GetByFollowerIdAndFolloweeIdAsync(Guid.Parse(followerId), Guid.Parse(followeeId));
    }

}

