using AutoMapper;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.File.Application.IServices;
using InfinityNetServer.Services.File.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.File.Application.GrpcServices
{
    public class GrpcFileService
        (ILogger<GrpcFileService> logger,
        IMinioClientService minioClientService,
        IPhotoMetadataService photoMetadataService,
        IVideoMetadataService videoMetadataService, IMapper mapper) : FileService.FileServiceBase
    {

        public override async Task<PhotoMetadataResponse> getPhotoMetadata(GetFileMetadataRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetPhotoMetadata called for {Id}", request.Id);
            PhotoMetadata source;
            try
            {
                source = await photoMetadataService.GetById(request.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting photo metadata for {Id}", request.Id);
                return new PhotoMetadataResponse();
            }
            PhotoMetadataResponse response = mapper.Map<PhotoMetadataResponse>(source);
            response.Url = await minioClientService.GetObjectUrl("infinity-net-bucket", response.Name);
            return response;
        }

        public override async Task<VideoMetadataResponse> getVideoMetadata(GetFileMetadataRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetVideoMetadata called for {Id}", request.Id);
            VideoMetadata source;
            try
            {
                source = await videoMetadataService.GetById(request.Id);
            }
            catch (Exception ex) {
                logger.LogError(ex, "Error getting video metadata for {Id}", request.Id);
                return new VideoMetadataResponse();
            }
            VideoMetadataResponse response = mapper.Map<VideoMetadataResponse>(source);
            response.Url = await minioClientService.GetObjectUrl("infinity-net-bucket", response.Name);
            response.Thumbnail.Url = await minioClientService.GetObjectUrl("infinity-net-bucket", response.Thumbnail.Name);
            return response;
        }

    }
}
