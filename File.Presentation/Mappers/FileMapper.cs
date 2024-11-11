using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.Services.File.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Presentation.Mappers;

public class FileMapper : AutoMapper.Profile
{
    public FileMapper()
    {
        CreateMap<PhotoMetadata, BuildingBlocks.Application.Protos.PhotoMetadataResponse>().ReverseMap();

        // tạo mapper
        // cú pháp <nguồn, đích>
        CreateMap<PhotoMetadata, PhotoMetadataResponse>()
            .AfterMap((src, dest) => {
                dest.Type = src.Type.ToString();
            }).ReverseMap();

        CreateMap<VideoMetadata, BuildingBlocks.Application.Protos.VideoMetadataResponse>().ReverseMap();

        // tạo mapper
        // cú pháp <nguồn, đích>
        CreateMap<VideoMetadata, VideoMetadataResponse>()
            .AfterMap((src, dest) => {
                dest.Type = src.Type.ToString();
            }).ReverseMap();
    }
}
