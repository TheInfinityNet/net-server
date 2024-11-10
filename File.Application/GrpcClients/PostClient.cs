using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.File.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.PostService;

namespace InfinityNetServer.Services.File.Application.GrpcClients
{
    public class PostClient(PostServiceClient client, ILogger<PostClient> logger)
    {

        public async Task<string> GetFileMetadataIdOfPost(string id)
        {
            try
            {
                logger.LogInformation("Starting get file metadata id of post");
                var response = await client.getFileMetadataIdOfPostAsync(new GetFileMetadataIdOfPostRequest { Id = id });
                // Call the gRPC server to introspect the token
                return response.FileMetadataId;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new FileException(FileErrorCode.FILE_EMPTY, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}
