using InfinityNetServer.Services.File.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Presentation.Services
{
    public class VideoMetadataService
        (ILogger<VideoMetadataService> logger, IVideoMetadataRepository videoMetadataRepository) : IVideoMetadataService
    {
        public async Task Create(VideoMetadata videoMetadata)
            => await videoMetadataRepository.CreateAsync(videoMetadata);

        public async Task Delete(string id)
        {
            VideoMetadata videoMetadata = await GetById(id);
            await videoMetadataRepository.DeleteAsync(videoMetadata);
        }

        public Task<VideoMetadata> GetById(string id)
            => videoMetadataRepository.GetByIdAsync(Guid.Parse(id));

        public async Task Update(VideoMetadata photoMetadata)
            => await videoMetadataRepository.UpdateAsync(photoMetadata);

    }
}
