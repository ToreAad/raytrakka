using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using RaytrAkkar.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RaytrAkkar.SceneRenderService
{
    class SceneRenderService
    {
        private ActorSystem _actorSystem;
        private IActorRef _sceneRenderSupervisor;

        public Task UntilTerminated => _actorSystem.WhenTerminated;

        public bool Start()
        {
            var config = ConfigurationFactory.Load();
            _actorSystem = ActorSystem.Create("raytrakkar", config);
            _sceneRenderSupervisor = _actorSystem.ActorOf(SceneRenderSupervisor.Props().WithRouter(FromConfig.Instance), "scene-render-supervisor");
            foreach (var _ in Enumerable.Range(0, 2))
            {
                _sceneRenderSupervisor.Tell(PoolPrimer.Instance);
            }
            return true;
        }

        public async Task Stop() => await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);
    }
}
