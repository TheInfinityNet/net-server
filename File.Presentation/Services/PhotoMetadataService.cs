using InfinityNetServer.Services.File.Application.Services;
using InfinityNetServer.Services.File.Domain.Entities;
using InfinityNetServer.Services.File.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Presentation.Services
{
    public class PhotoMetadataService
        (ILogger<PhotoMetadataService> logger, IPhotoMetadataRepository photoMetadataRepository) : IPhotoMetadataService
    {
        public async Task Create(PhotoMetadata photoMetadata)
            => await photoMetadataRepository.CreateAsync(photoMetadata);

        public async Task Delete(string id)
        {
            PhotoMetadata photoMetadata = await GetById(id);
            await photoMetadataRepository.SoftDeleteAsync(photoMetadata);
        }

        public Task<PhotoMetadata> GetById(string id)
            => photoMetadataRepository.GetByIdAsync(Guid.Parse(id));

        public async Task Update(PhotoMetadata photoMetadata)
            => await photoMetadataRepository.UpdateAsync(photoMetadata);

    }
}
