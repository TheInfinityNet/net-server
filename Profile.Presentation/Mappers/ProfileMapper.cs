using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Presentation.Mappers;

public class ProfileMapper : AutoMapper.Profile
{
    public ProfileMapper()
    {
        CreateMap<UserProfile, BuildingBlocks.Application.Protos.UserProfileResponse>();
    }
}
