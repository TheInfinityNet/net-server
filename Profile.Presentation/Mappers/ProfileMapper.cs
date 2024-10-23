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
                dest.Firstnameee = src.FirstName;
            }); ;
    }
}
