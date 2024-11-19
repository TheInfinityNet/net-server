using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IProfileBlockService
    {

        public Task<bool> HasBlocked(string currentProfileId, string targetProfileId);

        public Task<IList<string>> GetBlockerIds(string profileId, int? limit);

        public Task<IList<string>> GetBlockeeIds(string profileId, int? limit);

    }
}
