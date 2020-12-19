using Akka.Actor;
using Akka.Configuration;
using RaytrAkkar.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaytrAkkar.RaytracerService
{
    class RaytracerService
    {
        private ActorSystem _actorSystem;
        private IActorRef _raytracerService;

        public Task UntilTerminated => _actorSystem.WhenTerminated;

        public bool Start()
        {
            var config = ConfigurationFactory.Load();
            _actorSystem = ActorSystem.Create("raytrakkar", config);
            _raytracerService = _actorSystem.ActorOf(RaytracerActor.Props(), "raytracer");
            return true;
        }

        public async Task Stop() => await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);
    }
}
