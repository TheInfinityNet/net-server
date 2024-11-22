using System;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using InfinityNetServer.Services.Comment.Domain.Entities;
using AutoMapper;

namespace InfinityNetServer.Services.Comment.Presentation.Mappers
{
    public class CommentMapper : Profile
    {
        public CommentMapper()
        {
            CreateMap<InfinityNetServer.Services.Comment.Domain.Entities.Comment, CommentCountResponse>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId)) // Lấy ID bài viết từ Comment
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => 1)) // Đếm mỗi comment là 1
                .AfterMap((src, dest) =>
                {

                });
        }
    }
}
