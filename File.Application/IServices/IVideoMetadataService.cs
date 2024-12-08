using InfinityNetServer.Services.File.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.IServices
{
    public interface IVideoMetadataService
    {

        public Task Create(VideoMetadata videoMetadata);

        public Task Update(VideoMetadata videoMetadata);

        public Task SoftDelete(string id);

        public Task Delete(string id);

        public Task<VideoMetadata> GetById(string id);

    }
}
