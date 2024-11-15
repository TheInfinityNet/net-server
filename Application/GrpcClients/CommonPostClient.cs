﻿using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.PostService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonPostClient(PostServiceClient client, ILogger<CommonPostClient> logger, IMapper mapper)
    {

        public async Task<List<string>> GetPostIds()
        {
            try
            {
                logger.LogInformation("Starting get post ids");
                var response = await client.getPostIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<List<DTOs.Others.FileMetadataIdWithType>> GetFileMetadataIdsWithTypes(PostType type)
        {
            try
            {
                logger.LogInformation("Starting get file metadata ids with types");
                var response = await client.getFileMetadataIdsWithTypesAsync(new GetFileMetadataIdsWithTypesRequest { Type = type.ToString() });
                // Call the gRPC server to introspect the token
                return new List<DTOs.Others.FileMetadataIdWithType>(response.FileMetadataIdsWithTypes
                    .Select(_ => mapper.Map<DTOs.Others.FileMetadataIdWithType>(_)).ToList());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<DTOs.Responses.Post.PreviewPostResponse> GetPreviewPost(string id)
        {
            try
            {
                logger.LogInformation("Starting get preview post");
                var response = await client.getPreviewPostAsync(new PreviewPostRequest { Id = id });
                // Call the gRPC server to introspect the token
                return mapper.Map<DTOs.Responses.Post.PreviewPostResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.POST_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

    }
}
