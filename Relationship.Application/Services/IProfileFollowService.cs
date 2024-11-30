using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IProfileFollowService
    {

        Task<bool> HasFollowed(string currentProfileId, string targetProfileId);

        Task<IList<string>> GetAllFollowerIds(string currentProfileId);

        Task<IList<string>> GetAllFolloweeIds(string currentProfileId);
        Task<ProfileFollow> Follow(string followerId, string followeeId);
        Task<UnFollowResponse> UnFollow(string followId);
        Task<ProfileFollow> GetByFollowerIdAndFolloweeIdAsync(string followerId, string followeeId);

    }
}
