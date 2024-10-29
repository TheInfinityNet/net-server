using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Presentation.Mappers;

public class ProfileMapper : AutoMapper.Profile
{
    public ProfileMapper()
    {
        CreateMap<UserProfile, BuildingBlocks.Application.Protos.UserProfileResponse>();

        // tạo mapper
        // cú pháp <nguồn, đích>
        CreateMap<UserProfile, UserProfileResponse>()
            .AfterMap((src, dest) =>
            {
                //chỗ này custome nếu trg hợp đích (dest) và nguồn (src) khác tên thuộc tính
                dest.CoverId = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s";
                dest.AvatarId = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s";
            }); 
    }
}
