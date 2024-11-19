using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Services
{
    public interface IPostService
    {

        public Task<Domain.Entities.Post> GetById(string id);

        public Task<IList<Domain.Entities.Post>> GetByType(string type);

    }
}
