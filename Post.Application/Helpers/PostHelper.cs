using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using System.Collections.Generic;
using System.Linq;

namespace InfinityNetServer.Services.Post.Application.Helpers
{
    public class PostHelper
    {
        public static Domain.Entities.Post FromCreateRequest(CreatePostBaseRequest dto)
        {
            return new Domain.Entities.Post
            {
                //Content = dto.Content,
                //Type = dto.Type,
                //PresentationId = dto.PresentationId,
                //ParentId = dto.ParentId,
                //OwnerId = dto.OwnerId,
                //GroupId = dto.GroupId,
                //FileMetadataId = dto.FileMetadataId
            };
        }

        public static Domain.Entities.Post FromUpdateRequest(UpdatePostRequest dto)
        {
            return new Domain.Entities.Post
            {
                //Id = dto.Id,
                //Content = dto.Content,
                //Type = dto.Type,
                //PresentationId = dto.PresentationId,
                //ParentId = dto.ParentId,
                //OwnerId = dto.OwnerId,
                //GroupId = dto.GroupId,
                //FileMetadataId = dto.FileMetadataId
            };
        }

        public static PostResponse ToResponse(Domain.Entities.Post post)
        {
            if (post == null) return null;

            return new PostResponse
            {
                Id = post.Id,
                Content = post.Content,
                Type = post.Type,
                PresentationId = post.PresentationId,
                ParentId = post.ParentId,
                OwnerId = post.OwnerId,
                GroupId = post.GroupId,
                FileMetadataId = post.FileMetadataId,
            };
        }

        public static IEnumerable<PostResponse> ToResponses(IEnumerable<Domain.Entities.Post> posts)
        {
            if (posts == null) return null;

            return posts.Select(post => new PostResponse
            {
                Id = post.Id,
                Content = post.Content,
                Type = post.Type,
                PresentationId = post.PresentationId,
                ParentId = post.ParentId,
                OwnerId = post.OwnerId,
                GroupId = post.GroupId,
                FileMetadataId = post.FileMetadataId,
            });
        }

    }
}
