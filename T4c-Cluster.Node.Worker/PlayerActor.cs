using Akka.Persistence;

namespace T4c_Cluster.Node.PlayerWorker
{
    public class PlayerActor : ReceivePersistentActor
    {
        public override string PersistenceId => Self.Path.Name;
        public PlayerActor()
        {
            
        }

    }
}