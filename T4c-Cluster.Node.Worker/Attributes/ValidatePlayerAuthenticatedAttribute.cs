using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;

namespace T4c_Cluster.Node.Worker.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ValidatePlayerAuthenticatedAttribute : Attribute
    {
        public bool Validate(PlayerSession session) => session.IsAuthenticated;
    }
}
