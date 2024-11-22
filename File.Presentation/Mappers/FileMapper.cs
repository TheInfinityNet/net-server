using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.Services.File.Domain.Entities;

namespace InfinityNetServer.Services.File.Presentation.Mappers;

public class FileMapper : AutoMapper.Profile
{
    public FileMapper()
    {
        CreateMap<PhotoMetadata, BuildingBlocks.Application.Protos.PhotoMetadataResponse>();

        // tạo mapper
        // cú pháp <nguồn, đích>
        CreateMap<PhotoMetadata, PhotoMetadataResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();
            });

        CreateMap<VideoMetadata, BuildingBlocks.Application.Protos.VideoMetadataResponse>();

        // tạo mapper
        // cú pháp <nguồn, đích>
        CreateMap<VideoMetadata, VideoMetadataResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Type = src.Type.ToString();
            });
    }
}
