using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.IServices
{
    public interface IProfileService
    {

        public Task<Domain.Entities.Profile> GetById(string id);

        public Task<IList<Domain.Entities.Profile>> GetAllByIds(IList<string> ids);

        public Task<IList<Domain.Entities.Profile>> GetAll();

        public Task<IList<Domain.Entities.Profile>> GetAllByType(ProfileType type);

        public Task<IList<Domain.Entities.Profile>> GetPotentialByLocation(string location, int? limit);

        public Task<Domain.Entities.Profile> Update(Domain.Entities.Profile profile);

        public Task ConfirmSave(string id, string fileMetadataId, bool isAvatar);

        public Task<CursorPagedResult<Domain.Entities.Profile>> Search(string keywords, string profileId, string cursor, int limit);


    }
}
