using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Profile.Presentation.Mappers;

public class ProfileMapper : AutoMapper.Profile
{
    public ProfileMapper()
    {
        CreateMap<Domain.Entities.Profile, BuildingBlocks.Application.Protos.ProfileResponse>();

        CreateMap<Domain.Entities.Profile, BaseProfileResponse>();

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
                    dest.Avatar = new PhotoMetadataResponse
                    {
                        Id = Guid.Empty,
                        Name = "cover.jpg",
                        Width = 500,
                        Height = 500,
                        Size = 1000,
                        Type = FileMetadataType.Photo.ToString(),
                        Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                        CreatedAt = DateTime.Now
                    };
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
                    dest.Cover = new PhotoMetadataResponse
                    {
                        Id = Guid.Empty,
                        Name = "cover.jpg",
                        Width = 500,
                        Height = 500,
                        Size = 1000,
                        Type = FileMetadataType.Photo.ToString(),
                        Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                        CreatedAt = DateTime.Now
                    };
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
    }
}
