using InfinityNetServer.BuildingBlocks.Application.Consumers;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;

namespace InfinityNetServer.Services.Profile.Application.Consumers
{

    public class ActiveProfileConsumer(ISender sender) 
        : BaseConsumer<DomainCommand.ActiveProfileCommand>(sender)
    {


    }
}
