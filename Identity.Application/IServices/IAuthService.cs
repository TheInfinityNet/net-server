using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.IServices
{
    public interface IAuthService
    {

        public Task<Account> SignUp(SignUpRequest request, bool isActive, IMessageBus messageBus);

        public Task<Account> SignIn(string email, string password);

        public Task<bool> Introspect(string token);

        public string GenerateToken(Account account, Guid profileId, bool isRefresh);

        public Task<string> Refresh(string refreshToken);

        public Task SendMailWithCode(string toMail, VerificationType type, IMessageBus messageBus);

        public Task VerifyEmailByCode(string email, string code, IMessageBus messageBus);

        public string GenerateSocialAuthUrl(ProviderType providerType);

        public Task<Account> SocialCallback(string code, ProviderType providerType, IMessageBus messageBus);

    }
}
