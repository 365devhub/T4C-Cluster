using Akka.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Attributes;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;

namespace T4c_Cluster.Node.Worker.Controlers
{
    public static class ReceivePersistentActorExtentions
    {
        public static void RegisterControlerCommand<T, Session>(this ReceivePersistentActor self, T controler, Session session, Action saveSnapshotAction)
        {
            var interfaces = typeof(T).GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IControlerAction<,,>));
            var methods = typeof(T).GetMethods().Where(m => m.Name == "Action" && m.GetParameters().Count() == 3 && interfaces.Any(i => i.GenericTypeArguments.Count() == 3 && i.GenericTypeArguments[0] == m.GetParameters()[0].ParameterType && i.GenericTypeArguments[1] == m.GetParameters()[1].ParameterType && i.GenericTypeArguments[2] == m.GetParameters()[2].ParameterType) && m.GetParameters()[1].ParameterType == typeof(Session));


            foreach (var m in methods)
            {
                Type type0 = m.GetParameters()[0].ParameterType;
                Type type2 = m.GetParameters()[2].ParameterType;

                Action<object> handler = new Action<object>(o =>
                {
                    if (!(m.GetCustomAttribute<ValidatePlayerAuthenticatedAttribute>()?.Validate((PlayerSession)Convert.ChangeType(session, typeof(PlayerSession)))).GetValueOrDefault(true))
                        return;

                    if (!(m.GetCustomAttribute<ValidatePlayerNotAuthenticatedAttribute>()?.Validate((PlayerSession)Convert.ChangeType(session, typeof(PlayerSession)))).GetValueOrDefault(true))
                        return;



                    m.Invoke(controler, new object[] { Convert.ChangeType(o, type0), session, Convert.ChangeType(self, type2) });

                    if (m.GetCustomAttribute<SaveSnapshotAttribute>() != null)
                    {
                        saveSnapshotAction();
                    }


                });
                var command = typeof(ReceivePersistentActor).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                                            .Where(mi => mi.Name == "Command" && mi.GetParameters()[0].ParameterType == typeof(Type) && mi.GetParameters()[1].ParameterType == typeof(Action<object>) && mi.GetParameters()[2].ParameterType == typeof(Predicate<object>)).Single();

                command.Invoke(self, new object[] { type0, handler, (Predicate<object>)null });
            }
        }
    }
}
