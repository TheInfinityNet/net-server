using AutoMapper;
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
                throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<List<DTOs.Others.PreviewFileMetadata>> GetPreviewFileMetadatas(PostType type)
        {
            try
            {
                logger.LogInformation("Starting get file metadata ids with types");
                var response = await client.getPreviewFileMetadatasAsync(new PreviewFileMetadatasRequest { Type = type.ToString() });
                // Call the gRPC server to introspect the token
                return new List<DTOs.Others.PreviewFileMetadata>(response.PreviewFileMetadatas
                    .Select(mapper.Map<DTOs.Others.PreviewFileMetadata>).ToList());
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<DTOs.Responses.Post.PreviewPostResponse> GetPreviewPost(string id)
        {
            try
            {
                logger.LogInformation("Starting get preview post");
                var response = await client.getPreviewPostAsync(new PostRequest { Id = id });
                // Call the gRPC server to introspect the token
                return mapper.Map<DTOs.Responses.Post.PreviewPostResponse>(response);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);
            }
        }

        public async Task<List<string>> WhoCantSee(string id, string profileId)
        {
            try
            {
                logger.LogInformation("Starting who cant see");
                var response = await client.whoCantSeeAsync(new PostAudienceRequest { Id = id, ProfileId = profileId });
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                //throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);
                return [ profileId ];
            }
        }

    }
}
