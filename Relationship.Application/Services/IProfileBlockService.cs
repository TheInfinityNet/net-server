using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IProfileBlockService
    {

        Task<bool> HasBlocked(string currentProfileId, string targetProfileId);

    }
}
