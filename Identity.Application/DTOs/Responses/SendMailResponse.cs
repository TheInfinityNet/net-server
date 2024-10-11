namespace InfinityNetServer.Services.Identity.Application.DTOs.Responses
{
    public record SendMailResponse(

        string Message,

        int RetryAfter

    );
}
