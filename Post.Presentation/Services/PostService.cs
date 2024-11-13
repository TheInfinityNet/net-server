using InfinityNetServer.Services.Post.Application;
using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.Helpers;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Presentation.Services
{
    public class PostService(
            IPostRepository postRepository,
            IStringLocalizer<PostSharedResource> localizer,
            ILogger<PostService> logger) : IPostService
    {

        public async Task<Domain.Entities.Post> CreatePost(CreatePostRequest request)
        {
            Domain.Entities.Post requestPost = PostHelper.FromCreateRequest(request);
            return await postRepository.CreateAsync(requestPost);
        }

        public async Task<Domain.Entities.Post> DeletePost(string id)
        {
            await postRepository.DeleteAsync(Guid.Parse(id));
            return null;
        }

        public async Task<IEnumerable<Domain.Entities.Post>> GetAll()
        {
            return await postRepository.GetAllAsync();
        }

        public async Task<Domain.Entities.Post> GetById(string id)
        {
            return await postRepository.GetByIdAsync(Guid.Parse(id));
        }

        public async Task<Domain.Entities.Post> UpdatePost(UpdatePostRequest request)
        {
            Domain.Entities.Post requestPost = PostHelper.FromUpdateRequest(request);
            await postRepository.UpdateAsync(requestPost);
            return requestPost;
        }

        public async Task<IList<Domain.Entities.Post>> GetByType(string type)
            => await postRepository.GetByTypeAsync(Enum.Parse<PostType>(type));
        
    }
}
