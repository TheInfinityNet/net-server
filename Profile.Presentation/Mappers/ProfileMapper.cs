using InfinityNetServer.Services.Profile.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Presentation.Mappers;

public class ProfileMapper : AutoMapper.Profile
{
    public ProfileMapper()
    {
        CreateMap<UserProfile, BuildingBlocks.Application.Protos.UserProfileResponse>();

        // tạo mapper
        // cú pháp <nguồn, đích>
        CreateMap<UserProfile, MyInfoResponse>()
            .AfterMap((src, dest) =>
            {
                //chỗ này custome nếu trg hợp đích (dest) và nguồn (src) khác tên thuộc tính
                dest.CoverPhoto = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s";
                dest.AvatarPhoto = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s";
                dest.Status = "on";
            }); 
    }
}
