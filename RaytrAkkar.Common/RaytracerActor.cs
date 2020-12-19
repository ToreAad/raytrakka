using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Common
{
    public class RaytracerActor : UntypedActor
    {
        private readonly List<IActorRef> _renderedSceneListeners = new List<IActorRef>();
        private readonly List<IActorRef> _renderedTileListeners = new List<IActorRef>();
        private readonly IActorRef _sceneRenderSupervisor;

        public ILoggingAdapter Log { get; } = Context.GetLogger();
        public RaytracerActor()
        {
            _sceneRenderSupervisor = Context.ActorOf(SceneRenderSupervisor.Props().WithRouter(FromConfig.Instance), $"scene-render-supervisor");
        }
        protected override void PreStart() => Log.Info("RaytracerActor started");
        protected override void PostStop() => Log.Info("RaytracerActor stopped");

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    Log.Info($"Received render scene request from {Sender.Path}");
                    _sceneRenderSupervisor.Forward(scene);
                    break;
            }
        }
        
        public static Props Props() => Akka.Actor.Props.Create(() => new RaytracerActor());
    }
}
