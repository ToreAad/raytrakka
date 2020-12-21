using Akka.Actor;
using Akka.Bootstrap.Docker;
using Akka.Configuration;
using Akka.Routing;
using RaytrAkkar.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaytrAkkar.TileRenderService
{
    class TileRenderService
    {
        private ActorSystem _actorSystem;
        private IActorRef _tileRenderService;

        public Task UntilTerminated => _actorSystem.WhenTerminated;

        public bool Start()
        {
            var config = ConfigurationFactory.Load();
            _actorSystem = ActorSystem.Create("raytrakkar", config.BootstrapFromDocker());
            _tileRenderService = _actorSystem.ActorOf(TileRenderActor.Props().WithRouter(FromConfig.Instance), "tilerenderer");
            foreach(var _ in Enumerable.Range(0, 10))
            {
                _tileRenderService.Tell(PoolPrimer.Instance);
            }
            return true;
        }

        public async Task Stop() => await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);
    }
}
