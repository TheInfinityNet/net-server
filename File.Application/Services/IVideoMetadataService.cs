using InfinityNetServer.Services.File.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Services
{
    public interface IVideoMetadataService
    {

        Task Create(VideoMetadata videoMetadata);

        Task Update(VideoMetadata videoMetadata);

        Task Delete(string id);

        Task<VideoMetadata> GetById(string id);

    }
}
