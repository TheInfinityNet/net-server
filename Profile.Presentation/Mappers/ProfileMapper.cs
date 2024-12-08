using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Presentation.Mappers;

public class ProfileMapper : AutoMapper.Profile
{
    public ProfileMapper()
    {
        CreateMap<Domain.Entities.Profile, BuildingBlocks.Application.Protos.ProfileResponse>();

        CreateMap<Domain.Entities.Profile, BaseProfileResponse>();

        CreateMap<Domain.Entities.Profile, PreviewProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();
                //chỗ này custome nếu trg hợp đích (dest) và nguồn (src) khác tên thuộc tính
                if (src.AvatarId == null)
                {
                    dest.Avatar = null;
                }
                else
                {
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = src.AvatarId.Value,
                    };
                }
            });

        CreateMap<UserProfile, BuildingBlocks.Application.Protos.UserProfileResponse>();

        // tạo mapper
        // cú pháp <nguồn, đích>
        CreateMap<UserProfile, UserProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();
                //chỗ này custome nếu trg hợp đích (dest) và nguồn (src) khác tên thuộc tính
                if (src.AvatarId == null)
                {
                    dest.Avatar = null;
                }
                else
                {
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = src.AvatarId.Value,
                    };
                }

                if (src.CoverId == null)
                {
                    dest.Cover = null;
                }
                else
                {
                    dest.Cover = new PhotoMetadataResponse
                    {
                        Id = src.CoverId.Value,
                    };
                }
                dest.Name = dest.GenerateName();
            });

        CreateMap<PageProfile, PageProfileResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();
                //chỗ này custome nếu trg hợp đích (dest) và nguồn (src) khác tên thuộc tính
                if (src.AvatarId == null)
                {
                    dest.Avatar = null;
                }
                else
                {
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = src.AvatarId.Value,
                    };
                }

                if (src.CoverId == null)
                {
                    dest.Cover = null;
                }
                else
                {
                    dest.Cover = new PhotoMetadataResponse
                    {
                        Id = src.CoverId.Value,
                    };
                }
                dest.Name = dest.GenerateName();
            });

        // DTO -> Entity
        CreateMap<BaseProfileRequest, Domain.Entities.Profile>();

        CreateMap<UpdateUserProfileRequest, Domain.Entities.Profile>();

        CreateMap<UpdatePageProfileRequest, Domain.Entities.Profile>();

        CreateMap<UpdateUserProfileRequest, UserProfile>();

        CreateMap<UpdatePageProfileRequest, PageProfile>();
    }


}
