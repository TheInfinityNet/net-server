using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using InfinityNetServer.Services.Identity.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System;
using InfinityNetServer.Services.Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;
using InfinityNetServer.Services.Identity.Application.Exceptions;

namespace InfinityNetServer.Services.Identity.Presentation.Services
{
    public class AuthService : IAuthService
    {

        private readonly string ACCESS_SIGNER_KEY;

        private readonly string ACCESS_TOKEN_SIGNATURE_ALGORITHM = SecurityAlgorithms.HmacSha512;

        private readonly string REFRESH_SIGNER_KEY;

        private readonly string REFRESH_TOKEN_SIGNATURE_ALGORITHM = SecurityAlgorithms.HmacSha384;

        private readonly long VALID_DURATION;

        private readonly long REFRESHABLE_DURATION;

        private readonly IBaseRedisService<string, string> _baseRedisService;

        private readonly ILogger<AuthService> _logger;

        private readonly IConfiguration _configuration;

        public AuthService(
            IBaseRedisService<string, string> baseRedisService,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _baseRedisService = baseRedisService;
            _configuration = configuration;
            _logger = logger;

            ACCESS_SIGNER_KEY = _configuration["Jwt:AccessKey"]!;
            REFRESH_SIGNER_KEY = _configuration["Jwt:RefreshKey"]!;
            VALID_DURATION = long.Parse(_configuration["Jwt:ValidDuration"]!);
            REFRESHABLE_DURATION = long.Parse(_configuration["Jwt:RefreshDuration"]!);
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

        public string GenerateToken(Account user, bool isRefresh)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(isRefresh ? REFRESH_SIGNER_KEY : ACCESS_SIGNER_KEY));
            var credentials = new SigningCredentials(securityKey, isRefresh ? REFRESH_TOKEN_SIGNATURE_ALGORITHM : ACCESS_TOKEN_SIGNATURE_ALGORITHM);

            var expiryTime = isRefresh
                ? DateTime.UtcNow.AddSeconds(REFRESHABLE_DURATION)
                : DateTime.UtcNow.AddSeconds(VALID_DURATION);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]!),
                new Claim(JwtRegisteredClaimNames.Aud, "webapp.com")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiryTime,
                SigningCredentials = credentials
            };

            if (!isRefresh)
            {
                tokenDescriptor.Subject.AddClaim(new Claim("scope", "ADMIN"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> Refresh(string refreshToken, string accessToken)
        {
            // Verify the refresh token
            JwtSecurityToken signedJWT = await VerifyToken(refreshToken, true);
            string id = signedJWT.Subject ??
                throw new IdentityException(IdentityErrorCode.INVALID_TOKEN, StatusCodes.Status400BadRequest);

            Account user = new Account
            {
                AccountId = Guid.Parse("d55564f8-e09c-4d50-91c4-7d9d98b2f2d2")
            };

            if (!user.AccountId.Equals(Guid.Parse("d55564f8-e09c-4d50-91c4-7d9d98b2f2d2")))
                throw new IdentityException(IdentityErrorCode.INVALID_TOKEN, StatusCodes.Status400BadRequest);

            /*try
            {
                user = userService.FindByEmail(email); 
            }
            catch (AuthenticationException)
            {
                throw new AppException(AppErrorCode.INVALID_TOKEN, StatusCodes.Status400BadRequest);
            }*/

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(accessToken))
                throw new IdentityException(IdentityErrorCode.INVALID_TOKEN, StatusCodes.Status400BadRequest);

            var signedAccessToken = jwtHandler.ReadJwtToken(accessToken);
            var jwtID = signedAccessToken.Id;
            var expiryTime = signedAccessToken.ValidTo;

            var subject = signedAccessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (subject != id)
                throw new IdentityException(IdentityErrorCode.INVALID_TOKEN, StatusCodes.Status400BadRequest);

            if (expiryTime > DateTime.UtcNow)
            {
                var timeToLive = expiryTime.ToUniversalTime() - DateTime.UtcNow;
                await _baseRedisService.SetWithExpirationAsync(jwtID, "revoked", timeToLive);
            }

            return GenerateToken(user, false);
        }

        private async Task<JwtSecurityToken> VerifyToken(string token, bool isRefresh)
        {
            var key = Encoding.UTF8.GetBytes(isRefresh ? REFRESH_SIGNER_KEY : ACCESS_SIGNER_KEY);
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
                    if (expiryTime < DateTime.UtcNow) throw new IdentityException(IdentityErrorCode.TOKEN_EXPIRED, StatusCodes.Status401Unauthorized);

                    ValidateRefreshTokenSignature(token);
                }
                else if (expiryTime < DateTime.UtcNow) throw new IdentityException(IdentityErrorCode.TOKEN_INVALID, StatusCodes.Status401Unauthorized);

                var jwtID = jwtToken.Id;
                var value = await _baseRedisService.GetAsync(jwtID);

                if (value != null)
                {
                    if (value == "revoked") throw new IdentityException(IdentityErrorCode.TOKEN_REVOKED, StatusCodes.Status401Unauthorized);

                    else throw new IdentityException(IdentityErrorCode.TOKEN_BLACKLISTED, StatusCodes.Status401Unauthorized);
                }

                return (JwtSecurityToken)jwtToken;
            }
            catch (Exception)
            {
                throw new IdentityException(IdentityErrorCode.INVALID_SIGNATURE, StatusCodes.Status401Unauthorized);
            }

        }

        private void ValidateRefreshTokenSignature(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudiences = _configuration["Jwt:Audiences"]!.ToString().Split(",").Select(a => a.Trim()).ToArray(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(REFRESH_SIGNER_KEY)),
                ClockSkew = TimeSpan.Zero,
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch (Exception)
            {
                throw new IdentityException(IdentityErrorCode.INVALID_SIGNATURE, StatusCodes.Status401Unauthorized);
            }
        }

    }
}
