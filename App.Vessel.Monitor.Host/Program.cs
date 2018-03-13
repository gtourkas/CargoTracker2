using System;
using System.Threading.Tasks;
using Akka.Actor;
using App.Monitoring;
using MQTTnet;
using MQTTnet.Server;
using Infra.MongoDB;
using Infra.MongoDB.Monitoring.TempRangeMonitor;

namespace App.Vessel.Monitor.Host
{
    class Program
    {
        public static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("VesselMonitor");

            // internal event dispatcher
            var eventDispatcher = new EventDispatcher();

            // MongoDB as the Aggregate Repo
            var tempRangeMonitorRepo = new Repo(null);

            // create top-level actors within the actor system
            var tempRangeMonActorProps = TempRangeMonitorActor.Props(tempRangeMonitorRepo, eventDispatcher, "container1");
            var tempRangeMonActor = actorSystem.ActorOf(tempRangeMonActorProps, "container1");

            tempRangeMonActor.Tell(new TempRangeMonitorActor.TempReadingMessage() { ContainerId = "container1", Reading = 10, Timestamp = DateTime.Now});

            // MQTTNet impl of the MQTT listener
            // var mqttListener = new MQTTnetListener();
            // await mqttListener.Start();

            Console.WriteLine("press any key to end");
            Console.ReadKey();

            // await mqttListener.Stop();
        }
    }
}
