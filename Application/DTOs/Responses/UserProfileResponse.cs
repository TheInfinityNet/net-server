using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses
{
    public sealed class UserProfileResponse
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
    }
}
