using System;
using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public class InteractionService(
    IFriendshipService friendshipService,
    IInteractionRepository interactionRepository,
    ILogger<InteractionService> logger,
    IStringLocalizer<RelationshipSharedResource> localizer) : IInteractionService
    {

        private async Task<bool> HasFriendshipStatus(string currentProfileId, string targetProfileId, FriendshipStatus status) =>
            await friendshipService.GetByStatus(status, currentProfileId, targetProfileId) != null;

        private async Task<bool> HasInteractionType(string currentProfileId, string targetProfileId, InteractionType type) =>
            await interactionRepository.GetByTypeAsync(type, Guid.Parse(currentProfileId), Guid.Parse(targetProfileId)) != null;

        public async Task<bool> HasBlocked(string currentProfileId, string targetProfileId) =>
            await HasInteractionType(currentProfileId, targetProfileId, InteractionType.Block);

        public async Task<bool> HasFollowed(string currentProfileId, string targetProfileId) =>
            await HasInteractionType(currentProfileId, targetProfileId, InteractionType.Follow) &&
            await friendshipService.HasFriendship(currentProfileId, targetProfileId);

        public async Task<bool> HasMuted(string currentProfileId, string targetProfileId) =>
            await HasInteractionType(currentProfileId, targetProfileId, InteractionType.Mute) &&
            await friendshipService.HasFriendship(currentProfileId, targetProfileId);

        public Task<bool> HasFriendRequest(string currentProfileId, string targetProfileId) =>
            HasFriendshipStatus(targetProfileId, currentProfileId, FriendshipStatus.Pending);

        public async Task<IList<Interaction>> GetByType(
            InteractionType type, string currentProfileId, string targetProfileId, int? limit)
        {
            if (currentProfileId == null)
                return await interactionRepository.GetByRelateProfileAndTypeAsync(
                    type, Guid.Parse(targetProfileId), limit);

            else
                return await interactionRepository.GetByProfileAndTypeAsync(
                    type, Guid.Parse(currentProfileId), limit);

        }
    }

}

