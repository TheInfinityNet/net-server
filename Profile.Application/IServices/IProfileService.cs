using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
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

        public Task ConfirmSave(string id, string fileMetadataId, bool isAvatar, IMessageBus messageBus);


    }
}
