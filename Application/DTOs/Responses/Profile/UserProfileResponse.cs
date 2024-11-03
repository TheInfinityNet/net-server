using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile
{
    public sealed record UserProfileResponse : BaseProfileResponse
    {

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateOnly? Birthdate { get; set; }

        public string Gender { get; set; }

        public override string GenerateName()
        {
            return FirstName + " " + (MiddleName != null ? MiddleName + " " : "") + LastName;
        }
    }
}
