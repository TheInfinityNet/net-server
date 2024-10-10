using System;

namespace InfinityNetServer.BuildingBlocks.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class QueueNameAttribute : Attribute
    {
        public string QueueName { get; }

        public QueueNameAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }

}
