using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.Services.File.Application.IServices;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.Services
{
    public class VideoMetadataService
        (ILogger<VideoMetadataService> logger, IVideoMetadataRepository videoMetadataRepository) : IVideoMetadataService
    {
        public async Task Create(VideoMetadata videoMetadata)
            => await videoMetadataRepository.CreateAsync(videoMetadata);

        public async Task SoftDelete(string id)
        {
            VideoMetadata videoMetadata = await GetById(id) 
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);
            await videoMetadataRepository.SoftDeleteAsync(videoMetadata);
        }

        public async Task Delete(string id)
        {
            await videoMetadataRepository.DeleteAsync(Guid.Parse(id));
        }

        public Task<VideoMetadata> GetById(string id)
            => videoMetadataRepository.GetByIdAsync(Guid.Parse(id));

        public async Task Update(VideoMetadata photoMetadata)
        {
            var _ = await GetById(photoMetadata.Id.ToString())
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);
            await videoMetadataRepository.UpdateAsync(photoMetadata);
        }

    }
}
