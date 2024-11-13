using InfinityNetServer.Services.Post.Application.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Services
{
    public interface IPostService
    {

        public Task<Domain.Entities.Post> GetById(string id);

        public Task<Domain.Entities.Post> CreatePost(CreatePostRequest request);

        public Task<Domain.Entities.Post> UpdatePost(UpdatePostRequest request);

        public Task<IEnumerable<Domain.Entities.Post>> GetAll();

        public Task<Domain.Entities.Post> DeletePost(string id);

        public Task<IList<Domain.Entities.Post>> GetByType(string type);

    }
}
