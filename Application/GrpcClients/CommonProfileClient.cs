﻿using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static InfinityNetServer.BuildingBlocks.Application.Protos.ProfileService;

namespace InfinityNetServer.BuildingBlocks.Application.GrpcClients
{
    public class CommonProfileClient
    {

        private readonly ProfileServiceClient _client;

        private readonly ILogger<CommonProfileClient> _logger;

        public CommonProfileClient(ProfileServiceClient client, ILogger<CommonProfileClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<List<string>> GetProfileIds()
        {
            try
            {
                _logger.LogInformation("Starting get profile ids");
                var response = await _client.getProfileIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

        public async Task<List<string>> GetUserProfileIds()
        {
            try
            {
                _logger.LogInformation("Starting get user profile ids");
                var response = await _client.getUserProfileIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }
        public async Task<List<string>> GetPageProfileIds()
        {
            try
            {
                _logger.LogInformation("Starting get page profile ids");
                var response = await _client.getPageProfileIdsAsync(new Empty());
                // Call the gRPC server to introspect the token
                return new List<string>(response.Ids);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new CommonException(BaseErrorCode.SEED_DATA_ERROR, StatusCodes.Status422UnprocessableEntity);
            }
        }

    }
}