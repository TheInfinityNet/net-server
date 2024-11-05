using MassTransit;
using MediatR;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Messages
{

    [ExcludeFromTopology]
    public interface IMessage : IRequest
    {

        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

    }
}
