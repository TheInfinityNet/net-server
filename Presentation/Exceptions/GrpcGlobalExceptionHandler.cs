using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Exceptions;

public class GrpcGlobalExceptionHandler : Interceptor
{
    private readonly ILogger _logger;

    public GrpcGlobalExceptionHandler(ILogger logger)
    {
        _logger = logger;
    }

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
            _logger.LogError(exception, $"request : {JsonSerializer.Serialize(request)}");

            throw new RpcException(new Status(StatusCode.Cancelled, exception.Message));
        }
    }

}
