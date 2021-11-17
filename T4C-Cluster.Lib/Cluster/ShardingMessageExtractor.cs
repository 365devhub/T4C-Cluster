
using Akka.Cluster.Sharding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4C_Cluster.Lib.Cluster
{
    public class ShardingMessageExtractor : HashCodeMessageExtractor
    {
        public ShardingMessageExtractor() : base(1000)
        {
        }

        public override string EntityId(object message)
        {
            switch (message)
            {
                case ShardingEnvelope e: return e.EntityId;
                case ShardRegion.StartEntity start: return start.EntityId;
                default: return null;
            }
        }

        public override object EntityMessage(object message) => (message as ShardingEnvelope)?.Message ?? message;
    }
}
