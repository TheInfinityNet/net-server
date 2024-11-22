using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Presentation.Exceptions;

public class GrpcGlobalExceptionHandler(ILogger<GrpcGlobalExceptionHandler> logger) : Interceptor
{

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"request : {JsonSerializer.Serialize(request)}");

            throw new RpcException(new Status(StatusCode.Cancelled, exception.Message));
        }
    }

}
