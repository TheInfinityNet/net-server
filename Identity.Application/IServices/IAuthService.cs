using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using InfinityNetServer.Services.Identity.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.IServices
{
    public interface IAuthService
    {

        Task SignUp(SignUpRequest request, IMessageBus messageBus);

        Task<Account> SignIn(string email, string password);

        Task<bool> Introspect(string token);

        string GenerateToken(Account account, Guid profileId, bool isRefresh);

        Task<string> Refresh(string refreshToken);

        Task SendMailWithCode(string toMail, VerificationType type, IMessageBus messageBus);

        Task VerifyEmailByCode(string email, string code, IMessageBus messageBus);

    }
}
