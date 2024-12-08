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
    public class PhotoMetadataService
        (ILogger<PhotoMetadataService> logger, IPhotoMetadataRepository photoMetadataRepository) : IPhotoMetadataService
    {
        public async Task Create(PhotoMetadata photoMetadata)
            => await photoMetadataRepository.CreateAsync(photoMetadata);

        public async Task SoftDelete(string id)
        {
            PhotoMetadata photoMetadata = await GetById(id) 
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);
            await photoMetadataRepository.SoftDeleteAsync(photoMetadata);
        }

        public async Task Delete(string id)
        {
            await photoMetadataRepository.DeleteAsync(Guid.Parse(id));
        }

        public Task<PhotoMetadata> GetById(string id)
            => photoMetadataRepository.GetByIdAsync(Guid.Parse(id));

        public async Task Update(PhotoMetadata photoMetadata)
        {
            var _ = await GetById(photoMetadata.Id.ToString()) 
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);
            await photoMetadataRepository.UpdateAsync(photoMetadata);
        }

    }
}
