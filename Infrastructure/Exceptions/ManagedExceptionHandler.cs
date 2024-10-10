using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.Exceptions;

public class ManagedExceptionHandler<TRequest, TResponse, TException, IErrorCode>
    : IRequestExceptionHandler<TRequest, TResponse, TException> where TException : CommonException
{

    private readonly ILogger<ManagedExceptionHandler<TRequest, TResponse, TException, IErrorCode>> _logger;


    public ManagedExceptionHandler(ILogger<ManagedExceptionHandler<TRequest, TResponse, TException, IErrorCode>> logger)
    {
        _logger = logger;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (!string.IsNullOrEmpty(exception.Message)) _logger.LogError(exception, exception.Message);

        _logger.LogWarning(exception, $"request : {JsonSerializer.Serialize(request)}");

        state.SetHandled(default);

        return Task.CompletedTask;
    }

}
