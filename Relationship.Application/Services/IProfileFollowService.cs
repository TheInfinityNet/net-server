using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IProfileFollowService
    {

        Task<bool> HasFollowed(string currentProfileId, string targetProfileId);

        Task<IList<string>> GetAllFollowerIds(string currentProfileId, int? limit);

        Task<IList<string>> GetAllFolloweeIds(string currentProfileId, int? limit);

    }
}
