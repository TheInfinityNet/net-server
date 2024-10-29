using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses
{
    public sealed class UserProfileResponse : BaseProfileResponse
    {

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateOnly Birthdate { get; set; }

        public string Gender { get; set; }

        protected override void SetName()
        {
            Name = FirstName + " " + (MiddleName.Length != 0 ? MiddleName + " " : "") + LastName;
        }
    }
}
