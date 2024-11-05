using InfinityNetServer.Services.Post.Application;
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
        public async Task<Domain.Entities.Post> GetById(string id)
            => await postRepository.GetByIdAsync(Guid.Parse(id));
        

        public async Task<IList<Domain.Entities.Post>> GetByType(string type)
            => await postRepository.GetByTypeAsync(Enum.Parse<PostType>(type));
        
    }
}
