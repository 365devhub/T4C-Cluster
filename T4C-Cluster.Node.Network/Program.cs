using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.Configuration;
using Akka.DI.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Autofac.DependencyInjection;
using System;
using System.Threading;
using T4C_Cluster.Lib.Cluster;

namespace T4C_Cluster.Node.Network
{
    class Program
    {
        static void Main(string[] args)
        {

            var configuration = ReadConfiguration();

           

            var services = new ServiceCollection();
            PrepareServices(services, configuration);
            
            var providerFactory = new AutofacServiceProviderFactory();
            ContainerBuilder builder = providerFactory.CreateBuilder(services);
            PrepareContainer(builder, configuration);

            ActorSystem clusterSystem = null;
            builder.Register<ActorSystem>(c=> clusterSystem);
            var container = builder.Build();


            var akkaConfig = ConfigurationFactory.ParseString(configuration.GetSection("akka").GetValue<string>("hocon"));
            
            clusterSystem = ActorSystem.Create("ClusterSystem", akkaConfig);
            var region = ClusterSharding.Get(clusterSystem).StartProxy(typeName: "PlayerActor", role: null, messageExtractor: new ShardingMessageExtractor());
            
            clusterSystem.UseAutofac(container);

            var cluster = Cluster.Get(clusterSystem);

            var networkActor = clusterSystem.ActorOf(clusterSystem.DI().Props<NetworkActor>(), $"NetworkActor-{Guid.NewGuid().ToString()}");

            while (cluster.SelfMember.Status != MemberStatus.Up)
            {
                Thread.Sleep(100);
            }
            while (true)
            {
                Thread.Sleep(100);

            };
        }
        private static IConfigurationRoot ReadConfiguration()
        {
            return  new ConfigurationBuilder().AddXmlFile("App.config", optional: false, reloadOnChange: true)
                                                            .AddEnvironmentVariables().Build();
        }
        private static void PrepareServices(ServiceCollection services, IConfigurationRoot config)
        {
            services.AddHttpClient();
        }
        private static void PrepareContainer(ContainerBuilder builder, IConfigurationRoot config)
        {

            DateTime today = DateTime.Now.Date;
            var levelSwitch = new LoggingLevelSwitch() { MinimumLevel = Enum.Parse<LogEventLevel>(config.GetSection("Logging").GetValue<string>("MinimumLevel")) };
            var token = config.GetReloadToken().RegisterChangeCallback(x => levelSwitch.MinimumLevel = Enum.Parse<LogEventLevel>(config.GetValue<string>("MinimumLevel")), null);
            builder.RegisterSerilog(new LoggerConfiguration().MinimumLevel.ControlledBy(levelSwitch)
                                                             .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File($@"Logs\Info-{today.ToString("yyyy_MM_dd")}.log"))
                                                             .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File($@"Logs\Debug-{today.ToString("yyyy_MM_dd")}.log"))
                                                             .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File($@"Logs\Warning-{today.ToString("yyyy_MM_dd")}.log"))
                                                             .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File($@"Logs\Error-{today.ToString("yyyy_MM_dd")}.log"))
                                                             .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File($@"Logs\Fatal-{today.ToString("yyyy_MM_dd")}.log"))
                                                             .WriteTo.File($@"Logs\Verbose-{today.ToString("yyyy_MM_dd")}.log"));

            builder.Register(c => Options.Create(config.GetSection("Network").Get<Configuration.NetworkConfiguration>())).InstancePerDependency();



            builder.Register<LocalActorRef>(c=> (LocalActorRef)ClusterSharding.Get(c.Resolve<ActorSystem>()).ShardRegion("PlayerActor")).As<IActorRef>().Keyed<IActorRef>("RegionPlayerActor");
            builder.RegisterType<NetworkActor>();
        }

    }
}
