using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4c_Cluster.Node.Worker.Controllers
{
    public interface IControlerAction<TData, TSession>
    {
        void Action(TData data, TSession session, IActorRef actor);
    }
}
