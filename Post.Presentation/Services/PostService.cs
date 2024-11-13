using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using InfinityNetServer.Services.Post.Application.Helpers;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Presentation.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Domain.Entities.Post> CreatePost(CreatePostRequest request)
        {
            Domain.Entities.Post requestPost = PostHelper.FromCreateRequest(request);
            return await _postRepository.CreateAsync(requestPost);
        }

        public async Task<Domain.Entities.Post> DeletePost(string id)
        {
            await _postRepository.DeleteAsync(Guid.Parse(id));
            return null;
        }

        public async Task<IEnumerable<Domain.Entities.Post>> GetAll()
        {
            return await _postRepository.GetAllAsync();
        }

        public async Task<Domain.Entities.Post> GetById(string id)
        {
            return await _postRepository.GetByIdAsync(Guid.Parse(id));
        }

        public async Task<Domain.Entities.Post> UpdatePost(UpdatePostRequest request)
        {
            Domain.Entities.Post requestPost = PostHelper.FromUpdateRequest(request);
            await _postRepository.UpdateAsync(requestPost);
            return requestPost;
        }
    }
}
