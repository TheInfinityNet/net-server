using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application.Helpers;
using InfinityNetServer.Services.Identity.Application.Services;
using InfinityNetServer.Services.Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Presentation.Services
{
    public class AuthService : IAuthService
    {

        private readonly string _accessTokenSignatureAlgorithm = SecurityAlgorithms.HmacSha512;

        private readonly string _refreshTokenSignatureAlgorithm = SecurityAlgorithms.HmacSha384;

        private readonly JwtOptions _jwtOptions;

        private readonly IBaseRedisService<string, string> _baseRedisService;

        private readonly IAccountService _accountService;

        private readonly ILocalProviderService _localProviderService;

        private readonly ILogger<AuthService> _logger;

        private readonly IConfiguration _configuration;

        public AuthService(
            IBaseRedisService<string, string> baseRedisService,
            IAccountService accountService,
            ILocalProviderService localProviderService,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _baseRedisService = baseRedisService;
            _accountService = accountService;
            _localProviderService = localProviderService;
            _configuration = configuration;
            _jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
            _logger = logger;
        }

        public async Task<LocalProvider> SignIn(string email, string password)
        {
            var localProvider = await _localProviderService.GetByEmail(email);

            if (!PasswordHelper.VerifyPassword(localProvider.PasswordHash, password))
                throw new IdentityException(IdentityError.WRONG_PASSWORD, StatusCodes.Status401Unauthorized);

            return localProvider;
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

        public string GenerateToken(Account account, bool isRefresh)
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
                new Claim("profile_id", account.DefaultUserProfileId.ToString()),
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

        public async Task<string> Refresh(string refreshToken, string accessToken)
        {
            // Verify the refresh token
            JwtSecurityToken signedJwt = await VerifyToken(refreshToken, true);
            string id = signedJwt.Subject ??
                throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);

            Account account;
            try
            {
                account = await _accountService.GetById(id);
            }
            catch (Exception)
            {
                throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status404NotFound);
            }

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(accessToken))
                throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);

            var signedAccessToken = jwtHandler.ReadJwtToken(accessToken);
            var jwtId = signedAccessToken.Id;
            var expiryTime = signedAccessToken.ValidTo;

            var subject = signedAccessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (subject != id)
                throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);

            if (expiryTime > DateTime.UtcNow)
            {
                var timeToLive = expiryTime.ToUniversalTime() - DateTime.UtcNow;
                await _baseRedisService.SetWithExpirationAsync(jwtId, "revoked", timeToLive);
            }

            return GenerateToken(account, false);
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
                try
                {
                    await _accountService.GetById(subject);
                }
                catch (Exception)
                {
                    throw new IdentityException(IdentityError.INVALID_TOKEN, StatusCodes.Status401Unauthorized);
                }

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

    }
}
