using InfinityNetServer.Services.File.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.IServices
{
    public interface IVideoMetadataService
    {

        Task Create(VideoMetadata videoMetadata);

        Task Update(VideoMetadata videoMetadata);

        Task Delete(string id);

        Task<VideoMetadata> GetById(string id);

    }
}
