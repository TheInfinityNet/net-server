using System;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
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
                dest.Type = src.Type.ToString();
                //chỗ này custome nếu trg hợp đích (dest) và nguồn (src) khác tên thuộc tính
                dest.Cover = new PhotoMetadataResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Filename = "cover.jpg",
                    Width = 500,
                    Height = 500,
                    Size = 1000,
                    Type = FileMetadataType.Photo.ToString(),
                    Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                    CreatedAt = DateTime.Now
                };
                dest.Avatar = new PhotoMetadataResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Filename = "cover.jpg",
                    Width = 500,
                    Height = 500,
                    Size = 1000,
                    Type = FileMetadataType.Photo.ToString(),
                    Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                    CreatedAt = DateTime.Now
                };
                dest.Name = dest.GenerateName();
            }); 
    }
}
