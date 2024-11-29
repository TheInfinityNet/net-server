namespace InfinityNetServer.Services.Comment.Presentation.Mappers;

public class CommentMappers : AutoMapper.Profile
{
    public CommentMappers()
    {
        CreateMap<BuildingBlocks.Application.DTOs.Responses.Comment.CommentPreviewResponse.TagFacetResponse, BuildingBlocks.Application.Protos.TagFacetResponse>();

        CreateMap<BuildingBlocks.Application.DTOs.Responses.Comment.CommentPreviewResponse.ContentResponse, BuildingBlocks.Application.Protos.ContentResponse>();

        CreateMap<BuildingBlocks.Application.DTOs.Responses.Comment.CommentPreviewResponse, BuildingBlocks.Application.Protos.CommentPreviewResponse>();
    }
}
