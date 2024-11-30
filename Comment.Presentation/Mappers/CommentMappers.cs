using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using System;
namespace InfinityNetServer.Services.Comment.Presentation.Mappers;

public class CommentMappers : AutoMapper.Profile
{
    public CommentMappers()
    {

        CreateMap<Domain.Entities.CommentContent, BuildingBlocks.Application.Protos.CommentContent>();

        CreateMap<Domain.Entities.Comment, BuildingBlocks.Application.Protos.PreviewCommentResponse>();

        CreateMap<Domain.Entities.Comment, BuildingBlocks.Application.Protos.CommentResponse>();

        CreateMap<Domain.Entities.Comment, CommentResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Profile = new PreviewProfileResponse { Id = src.ProfileId };
                //dest.Type = src.Type.ToString();
                if (src.FileMetadataId != null)
                    dest.Photo = new PhotoMetadataResponse { Id = src.FileMetadataId.Value };
            });

        // DTO -> Entity
        CreateMap<BuildingBlocks.Application.DTOs.Others.CommentContent, Domain.Entities.CommentContent>();

        CreateMap<CommentBaseRequest, Domain.Entities.Comment>();

    }
}
