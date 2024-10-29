using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application
{
    public interface IFriendshipService
    {

        Task<int> CountFriendships(string profileId);

        Task<IList<string>> GetPreviewFriendIds(string profileId);

    }
}
