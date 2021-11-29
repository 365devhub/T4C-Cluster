using Akka.Actor;
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
        public static void RegisterControlerCommand<T, Session>(this ReceivePersistentActor actor, T controler, Session session, Action saveSnapshotAction, IActorRef self)
        {

            var interfaces = typeof(T).GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IControlerAction<,>));
            var methods = typeof(T).GetMethods().Where(m => m.Name == "Action" && 
                                                            m.GetParameters().Count() == 3 && 
                                                            interfaces.Any(i => i.Name == typeof(IControlerAction<,>).Name && 
                                                                                i.GenericTypeArguments.Count() == 2 && 
                                                                                i.GenericTypeArguments[0] == m.GetParameters()[0].ParameterType && 
                                                                                i.GenericTypeArguments[1] == m.GetParameters()[1].ParameterType) && m.GetParameters()[1].ParameterType == typeof(Session));


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
                    if (!(m.GetCustomAttribute<ValidatePlayerNotInGameAttribute>()?.Validate((PlayerSession)Convert.ChangeType(session, typeof(PlayerSession)))).GetValueOrDefault(true))
                        return;
                    if (!(m.GetCustomAttribute<ValidatePlayerInGameAttribute>()?.Validate((PlayerSession)Convert.ChangeType(session, typeof(PlayerSession)))).GetValueOrDefault(true))
                        return;

                    var message = Convert.ChangeType(o, type0);
                    m.Invoke(controler, new object[] { message, session, self });

                    if (m.GetCustomAttribute<SaveSnapshotAttribute>() != null)
                    {
                        saveSnapshotAction();
                    }


                });

                var commands = typeof(ReceivePersistentActor).GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(mi => mi.Name == "Command");
                var command = commands.Where (mi=> mi.GetParameters()[0].ParameterType == typeof(Type) && 
                                                                         mi.GetParameters()[1].ParameterType == typeof(Action<object>) && 
                                                                         mi.GetParameters()[2].ParameterType == typeof(Predicate<object>)).Single();

                command.Invoke(actor, new object[] { type0, handler, (Predicate<object>)null });
            }
        }
    }
}
