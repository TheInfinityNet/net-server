using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonFileClient(FileService.FileServiceClient client, ILogger<CommonFileClient> logger, IMapper mapper)
    {

        public async Task<DTOs.Responses.File.PhotoMetadataResponse> GetPhotoMetadata(string id)
        {
            try
            {
                logger.LogInformation("Starting get photo metadata");
                var response = await client.getPhotoMetadataAsync(new GetFileMetadataRequest
                {
                    Id = id
                });
                return mapper.Map<DTOs.Responses.File.PhotoMetadataResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new CommonException(BaseErrorCode.FILE_NOT_FOUND, StatusCodes.Status404NotFound);
                return null;
            }
        }

        public async Task<DTOs.Responses.File.VideoMetadataResponse> GetVideoMetadata(string id)
        {
            try
            {
                logger.LogInformation("Starting get video metadata");
                var response = await client.getVideoMetadataAsync(new GetFileMetadataRequest
                {
                    Id = id
                });
                return mapper.Map<DTOs.Responses.File.VideoMetadataResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new CommonException(BaseErrorCode.FILE_NOT_FOUND, StatusCodes.Status404NotFound);
                return null;
            }
        }

    }
}
