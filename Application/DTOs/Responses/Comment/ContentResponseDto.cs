// Tạo DTO cho ContentResponse
using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment
{
    public class ContentResponseDto
    {
        public string Text { get; set; }
        public List<TagFacetResponseDto> TagFacets { get; set; }
    }

    public class TagFacetResponseDto
    {
        public string Type { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string ProfileId { get; set; }
    }
}
