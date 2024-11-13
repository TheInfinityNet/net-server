using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Helpers
{
    public class PostHelper
    {
        public static Domain.Entities.Post FromCreateRequest(CreatePostRequest dto)
        {
            return new Domain.Entities.Post
            {
                Content = dto.Content,
                Type = dto.Type,
                PresentationId = dto.PresentationId,
                ParentId = dto.ParentId,
                OwnerId = dto.OwnerId,
                GroupId = dto.GroupId,
                FileMetadataId = dto.FileMetadataId
            };
        }

        public static Domain.Entities.Post FromUpdateRequest(UpdatePostRequest dto)
        {
            return new Domain.Entities.Post
            {
                Id = dto.Id,
                Content = dto.Content,
                Type = dto.Type,
                PresentationId = dto.PresentationId,
                ParentId = dto.ParentId,
                OwnerId = dto.OwnerId,
                GroupId = dto.GroupId,
                FileMetadataId = dto.FileMetadataId
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
                SharedPosts = MapSharedPosts(post.SharedPosts),
                SubPosts = MapSubPosts(post.SubPosts),
                PostPrivacies = post.PostPrivacies
            };
        }

        private static ICollection<PostResponse> MapSharedPosts(ICollection<Domain.Entities.Post> sharedPosts)
        {
            var result = new List<PostResponse>();
            if (sharedPosts == null) return result;

            foreach (var sharedPost in sharedPosts)
            {
                result.Add(ToResponse(sharedPost));
            }

            return result;
        }

        private static ICollection<PostResponse> MapSubPosts(ICollection<Domain.Entities.Post> subPosts)
        {
            var result = new List<PostResponse>();
            if (subPosts == null) return result;

            foreach (var subPost in subPosts)
            {
                result.Add(ToResponse(subPost));
            }

            return result;
        }

        private static ICollection<PostPrivacyResponse> MapPostPrivacies(ICollection<PostPrivacy> postPrivacies)
        {
            var result = new List<PostPrivacyResponse>();
            if (postPrivacies == null) return result;

            foreach (var privacy in postPrivacies)
            {
                result.Add(new PostPrivacyResponse
                {
                    Id = privacy.Id,
                    PostId = privacy.PostId,
                    Type = privacy.Type
                });
            }

            return result;
        }
    }
}
