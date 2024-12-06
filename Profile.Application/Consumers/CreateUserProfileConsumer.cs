using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Profile.Application.Consumers
{

    public class CreateUserProfileConsumer(ISender sender) 
        : BaseConsumer<DomainCommand.CreateUserProfileCommand>(sender)
    {


    }
}
