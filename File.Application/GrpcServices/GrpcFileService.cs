using AutoMapper;
using Grpc.Core;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.File.Application.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.GrpcServices
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
            var source = await photoMetadataService.GetById(request.Id);
            PhotoMetadataResponse response = mapper.Map<PhotoMetadataResponse>(source);
            response.Url = await minioClientService.GetObjectUrl("infinity-net-bucket", response.Name);
            return response;
        }

        public override async Task<VideoMetadataResponse> getVideoMetadata(GetFileMetadataRequest request, ServerCallContext context)
        {
            logger.LogInformation("GetPhotoMetadata called for {Id}", request.Id);
            var source = await videoMetadataService.GetById(request.Id);
            VideoMetadataResponse response = mapper.Map<VideoMetadataResponse>(source);
            response.Url = await minioClientService.GetObjectUrl("infinity-net-bucket", response.Name);
            response.Thumbnail.Url = await minioClientService.GetObjectUrl("infinity-net-bucket", response.Thumbnail.Name);
            return response;
        }

    }
}
