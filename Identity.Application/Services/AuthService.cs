using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Identity.Application.DTOs.Requests;
using InfinityNetServer.Services.Identity.Application.DTOs.Responses;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application.Helpers;
using InfinityNetServer.Services.Identity.Application.IServices;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly string _accessTokenSignatureAlgorithm = SecurityAlgorithms.HmacSha512;

        private readonly string _refreshTokenSignatureAlgorithm = SecurityAlgorithms.HmacSha384;

        private readonly JwtOptions _jwtOptions;

        private readonly SocialOauth2Options _googleOptions;

        private readonly SocialOauth2Options _facebookOptions;

        private readonly HttpClient _httpClient;

        private readonly IBaseRedisService<string, string> _baseRedisService;

        private readonly IAccountService _accountService;

        private readonly ILocalProviderService _localProviderService;

        private IGoogleProviderRepository _googleProviderRepository;

        private IFacebookProviderRepository _facebookProviderRepository;

        private readonly IVerificationService _verificationService;

        private readonly ILogger<AuthService> _logger;

        private readonly IConfiguration _configuration;

        public AuthService(
            IBaseRedisService<string, string> baseRedisService,
            IAccountService accountService,
            ILocalProviderService localProviderService,
            IGoogleProviderRepository googleProviderRepository,
            IFacebookProviderRepository facebookProviderRepository,
            IVerificationService verificationService,
            IConfiguration configuration,
            HttpClient httpClient,
            ILogger<AuthService> logger)
        {
            _baseRedisService = baseRedisService;
            _accountService = accountService;
            _localProviderService = localProviderService;
            _verificationService = verificationService;
            _googleProviderRepository = googleProviderRepository;
            _facebookProviderRepository = facebookProviderRepository;
            _configuration = configuration;
            _jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
            _googleOptions = _configuration.GetSection("SocialOauth2:Google").Get<SocialOauth2Options>();
            _facebookOptions = _configuration.GetSection("SocialOauth2:Facebook").Get<SocialOauth2Options>();
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger;
        }

        public async Task<Account> SignIn(string email, string password)
        {
            var localProvider = await _localProviderService.GetByEmail(email)
                ?? throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status404NotFound);

            if (!PasswordHelper.VerifyPassword(localProvider.PasswordHash, password))
                throw new IdentityException(IdentityError.WRONG_PASSWORD, StatusCodes.Status401Unauthorized);

            return await _accountService.GetById(localProvider.AccountId.ToString());
        }

        public async Task<bool> Introspect(string token)
        {
            bool isValid = true;

            try
            {
                await VerifyToken(token, false);
            }
            catch (Exception)
            {
                isValid = false;
            }
            return isValid;
        }

        public string GenerateToken(Account account, Guid profileId, bool isRefresh)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(isRefresh ? _jwtOptions.RefreshKey! : _jwtOptions.AccessKey));

            var credentials = new SigningCredentials(
                securityKey, isRefresh ? _refreshTokenSignatureAlgorithm : _accessTokenSignatureAlgorithm);

            var expiryTime = isRefresh
                ? DateTime.UtcNow.AddSeconds(double.Parse(_jwtOptions.RefreshDuration.ToString()))
                : DateTime.UtcNow.AddSeconds(double.Parse(_jwtOptions.ValidDuration.ToString()));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("profile_id", profileId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtOptions.Audiences.First())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiryTime,
                SigningCredentials = credentials
            };

            if (!isRefresh) tokenDescriptor.Subject.AddClaim(new Claim("scope", "User"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> Refresh(string refreshToken)
        {
            JwtSecurityToken signedJwt = await VerifyToken(refreshToken, true);

            string accountId = signedJwt.Subject ??
                throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);
            Guid profileId = Guid.Parse(signedJwt.Claims.FirstOrDefault(c => c.Type == "profile_id")?.Value
                ?? throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized));
            Account account = await _accountService.GetById(accountId)
                ?? throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status404NotFound);

            return GenerateToken(account, profileId, false);
        }

        private async Task<JwtSecurityToken> VerifyToken(string token, bool isRefresh)
        {
            var key = Encoding.UTF8.GetBytes(isRefresh ? _jwtOptions.RefreshKey! : _jwtOptions.AccessKey);

            var securityKey = new SymmetricSecurityKey(key);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero, // Remove default 5 min clock skew for token expiry
                ValidateLifetime = !isRefresh, // Refresh tokens may require special handling for expiry checks
            };

            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                var jwtToken = validatedToken;
                var expiryTime = jwtToken.ValidTo;

                if (isRefresh)
                {
                    if (expiryTime < DateTime.UtcNow) throw new IdentityException(IdentityError.TOKEN_EXPIRED, StatusCodes.Status401Unauthorized);

                    ValidateRefreshTokenSignature(token);
                }
                else if (expiryTime < DateTime.UtcNow) throw new BaseException(BaseError.TOKEN_INVALID, StatusCodes.Status401Unauthorized);

                var jwtId = jwtToken.Id;
                var value = await _baseRedisService.GetAsync(jwtId);

                if (value != null)
                {
                    if (value == "revoked") throw new IdentityException(IdentityError.TOKEN_REVOKED, StatusCodes.Status401Unauthorized);

                    else throw new IdentityException(IdentityError.TOKEN_BLACKLISTED, StatusCodes.Status401Unauthorized);
                }

                var subject = ((JwtSecurityToken)jwtToken).Subject ??
                                throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);

                var _ = await _accountService.GetById(subject)
                    ?? throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);

                return (JwtSecurityToken)jwtToken;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new BaseException(BaseError.INVALID_SIGNATURE, StatusCodes.Status401Unauthorized);
            }

        }

        private void ValidateRefreshTokenSignature(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudiences = jwtOptions.Audiences,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.RefreshKey!)),
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch (Exception)
            {
                throw new BaseException(BaseError.INVALID_SIGNATURE, StatusCodes.Status401Unauthorized);
            }
        }

        public async Task SendMailWithCode(string toMail, VerificationType type, IMessageBus messageBus)
        {

            LocalProvider localProvider = await _localProviderService.GetByEmail(toMail)
                ?? throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status404NotFound);

            string secret = GenerateVerificationCode(6);
            Verification verification = await _verificationService.Create(new Verification
            {
                Code = secret,
                Token = $"{Guid.NewGuid()}???{secret}",
                ExpiresAt = DateTime.Now.AddMinutes(5),
                AccountId = localProvider.Account.Id,
                CreatedBy = localProvider.Account.Id
            });

            await messageBus.Publish(new DomainCommand.SendMailWithCodeCommand
            {
                Id = Guid.NewGuid(),
                ToMail = toMail,
                Type = type,
                AcceptLanguage = CultureInfo.CurrentCulture.ToString(),
                Code = verification.Code,
                CreatedAt = DateTime.Now,
                CreatedBy = verification.CreatedBy
            });
        }

        public async Task<Account> SignUp(SignUpRequest request, bool isActive, IMessageBus messageBus)
        {

            if (request.PasswordConfirmation != request.Password)
                throw new IdentityException(IdentityError.PASSWORD_MISMATCH, StatusCodes.Status400BadRequest);

            if (!request.AcceptTerms)
                throw new IdentityException(IdentityError.TERMS_NOT_ACCEPTED, StatusCodes.Status400BadRequest);

            if (request.Birthdate.Year < 1900 || request.Birthdate > DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
                throw new IdentityException(IdentityError.INVALID_BIRTHDATE, StatusCodes.Status400BadRequest);

            LocalProvider existingLocalProvider = await _localProviderService.GetByEmail(request.Email);

            if (existingLocalProvider != null)
                throw new IdentityException(IdentityError.EMAIL_ALREADY_IN_USE, StatusCodes.Status400BadRequest);

            Account account = await _accountService.Create(new()
            {
                DefaultUserProfileId = Guid.NewGuid(),
                AccountProviders = [
                    new LocalProvider()
                    {
                        Email = request.Email,
                        PasswordHash = PasswordHelper.HashPassword(request.Password),
                    }]
            });

            await messageBus.Publish(new DomainCommand.CreateUserProfileCommand
            {
                AccountId = account.Id,
                ProfileId = account.DefaultUserProfileId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                MobileNumber = request.MobileNumber,
                Birthdate = request.Birthdate,
                Gender = Enum.Parse<Gender>(request.Gender),
                IsActive = isActive
            });

            return account;
        }

        public async Task VerifyEmailByCode(string email, string code, IMessageBus messageBus)
        {
            LocalProvider localProvider = await _localProviderService.GetByEmail(email)
                ?? throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status404NotFound); ;
            Verification verification = await _verificationService.GetByCodeAndAccountId(code, localProvider.AccountId.ToString())
                ?? throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status404NotFound);

            if (verification.ExpiresAt < DateTime.Now)
                throw new IdentityException(IdentityError.INVALID_ACTIVATION_CODE, StatusCodes.Status400BadRequest);

            await messageBus.Publish(new DomainCommand.ActiveProfileCommand
            {
                Id = Guid.NewGuid(),
                ProfileId = localProvider.Account.DefaultUserProfileId,
            });

            await _verificationService.Delete(verification.Id.ToString());
        }

        public string GenerateSocialAuthUrl(ProviderType providerType)
        {
            return providerType switch
            {
                ProviderType.Google =>
                    $"{_googleOptions.AuthUri}" +
                    $"?client_id={_googleOptions.ClientId}" +
                    $"&redirect_uri={_googleOptions.RedirectUri}" +
                    $"&scope={(_googleOptions.Scopes.Length > 0 ? string.Join(" ", _googleOptions.Scopes) : "openid")}&response_type=code",

                ProviderType.Facebook =>
                    $"{_facebookOptions.AuthUri}" +
                    $"?client_id={_facebookOptions.ClientId}" +
                    $"&redirect_uri={_facebookOptions.RedirectUri}" +
                    $"&scope={(_facebookOptions.Scopes.Length > 0 ? string.Join(",", _facebookOptions.Scopes) : "email")}&response_type=code",

                _ => throw new IdentityException(IdentityError.INVALID_PROVIDER, StatusCodes.Status400BadRequest)
            };
        }

        public async Task<Account> SocialCallback(string code, ProviderType providerType, IMessageBus messageBus)
        {
            // Fetch user info from social provider
            var userInfo = await FetchSocialUserAsync(code, providerType)
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound); ;
            string email = userInfo.GetValueOrDefault("email")?.ToString()
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);
            string name = userInfo.GetValueOrDefault("name")?.ToString()
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            // Check if user already exists
            var randomPassword = PasswordHelper.HashPassword(_googleOptions.ClientSecret + _facebookOptions.ClientSecret);
            var nameParts = name.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            LocalProvider existingLocalProvider = await _localProviderService.GetByEmail(email);
            Account existingAccount = existingLocalProvider?.Account;

            if (existingLocalProvider == null)
            {
                // Create new user
                var signUpRequest = new SignUpRequest
                {
                    Email = email,
                    FirstName = nameParts.ElementAtOrDefault(0),
                    LastName = nameParts.ElementAtOrDefault(1) ?? nameParts.ElementAtOrDefault(0),
                    MobileNumber = "not provided",
                    Gender = Gender.Other.ToString(),
                    Birthdate = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)),
                    Password = randomPassword,
                    PasswordConfirmation = randomPassword,
                    AcceptTerms = true
                };

                existingAccount = await SignUp(signUpRequest, true, messageBus);
                
                switch (providerType)
                {
                    case ProviderType.Google:
                        await _googleProviderRepository.CreateAsync(new GoogleProvider
                        {
                            AccountId = existingAccount.Id,
                            CreatedBy = existingAccount.Id,
                            GoogleId = Guid.NewGuid(),
                        });
                        break;

                    case ProviderType.Facebook:
                        await _facebookProviderRepository.CreateAsync(new FacebookProvider
                        {
                            AccountId = existingAccount.Id,
                            CreatedBy = existingAccount.Id,
                            FacebookId = Guid.NewGuid(),
                        });
                        break;
                }
            }
            else
            {
                switch (providerType)
                {
                    case ProviderType.Google:
                        var googleProvider = await _googleProviderRepository.GetByAccountIdAsync(existingAccount.Id);
                        if (googleProvider == null)
                        {
                            _logger.LogInformation("Google provider not found for existing account.");
                            await _googleProviderRepository.CreateAsync(new GoogleProvider
                            {
                                AccountId = existingAccount.Id,
                                CreatedBy = existingAccount.Id,
                                GoogleId = Guid.NewGuid(),
                            });
                        }
                        break;

                    case ProviderType.Facebook:
                        var facebookProvider = await _facebookProviderRepository.GetByAccountIdAsync(existingAccount.Id);
                        if (facebookProvider == null)
                        {
                            await _facebookProviderRepository.CreateAsync(new FacebookProvider
                            {
                                AccountId = existingAccount.Id,
                                CreatedBy = existingAccount.Id,
                                FacebookId = Guid.NewGuid(),
                            });
                        }
                        break;
                }
            }
            // Authenticate user
            var signInUser = existingAccount;
            return signInUser;
        }

        private async Task<Dictionary<string, object>> FetchSocialUserAsync(string code, ProviderType providerType)
        {
            string accessToken;
            switch (providerType)
            {
                case ProviderType.Google:
                    accessToken = await GetGoogleAccessTokenAsync(code);
                    return await FetchGoogleUserInfoAsync(accessToken);

                case ProviderType.Facebook:
                    accessToken = await GetFacebookAccessTokenAsync(code);
                    return await FetchFacebookUserInfoAsync(accessToken);

                default:
                    throw new IdentityException(IdentityError.INVALID_PROVIDER, StatusCodes.Status400BadRequest);
            }
        }

        private async Task<string> GetGoogleAccessTokenAsync(string code)
        {
            var parameters = new Dictionary<string, string>
            {
                { "client_id", _googleOptions.ClientId },
                { "client_secret", _googleOptions.ClientSecret },
                { "code", code },
                { "redirect_uri", _googleOptions.RedirectUri },
                { "grant_type", "authorization_code" }
            };

            var response = await _httpClient.PostAsync(_googleOptions.TokenUri, new FormUrlEncodedContent(parameters));
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (jsonResponse.TryGetProperty("access_token", out var accessToken))
            {
                return accessToken.GetString();
            }

            _logger.LogError("Failed to fetch Google access token.");
            throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);
        }

        private async Task<Dictionary<string, object>> FetchGoogleUserInfoAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>(_googleOptions.UserInfoUri) 
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);
            return response;
        }

        private async Task<string> GetFacebookAccessTokenAsync(string code)
        {
            var url = $"{_facebookOptions.TokenUri}" +
                $"?client_id={_facebookOptions.ClientId}" +
                $"&client_secret={_facebookOptions.ClientSecret}" +
                $"&redirect_uri={_facebookOptions.RedirectUri}&code={code}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (jsonResponse.TryGetProperty("access_token", out var accessToken))
            {
                return accessToken.GetString();
            }

            _logger.LogError("Failed to fetch Facebook access token.");
            throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);
        }

        private async Task<Dictionary<string, object>> FetchFacebookUserInfoAsync(string accessToken)
        {
            var url = $"{_facebookOptions.UserInfoUri}?access_token={accessToken}";
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>(url) 
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);
            return response;
        }

        private static string GenerateVerificationCode(int length)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder code = new ();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[1];

                for (int i = 0; i < length; i++)
                {
                    int randomIndex;
                    do
                    {
                        rng.GetBytes(buffer);
                        randomIndex = buffer[0] % characters.Length;
                    } while (randomIndex < 0);

                    code.Append(characters[randomIndex]);
                }
            }

            return code.ToString();
        }

    }
}
