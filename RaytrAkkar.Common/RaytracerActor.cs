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
            _sceneRenderSupervisor = Context.ActorOf(SceneRenderSupervisor.Props(Self).WithRouter(FromConfig.Instance), $"scene-render-supervisor");
        }
        protected override void PreStart() => Log.Info("RaytracerActor started");
        protected override void PostStop() => Log.Info("RaytracerActor stopped");

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    _sceneRenderSupervisor.Tell(scene);
                    foreach (var listener in _renderedSceneListeners)
                    {
                        listener.Tell(scene);
                    }
                    break;
                case RenderedScene renderedScene:
                    foreach(var listener in _renderedSceneListeners)
                    {
                        listener.Tell(renderedScene);
                    }
                    break;
                case RenderedTile renderedTile:
                    foreach (var listener in _renderedTileListeners)
                    {
                        listener.Tell(renderedTile);
                    }
                    break;
                case RegisterRenderedSceneListener listener:
                    Log.Info($"RegisterRenderedSceneListener registered by {Sender.Path}");
                    _renderedSceneListeners.Add(listener.actor);
                    listener.actor.Tell(new Received());
                    break;
                case UnregisterRenderedSceneListener listener:
                    Log.Info($"UnregisterRenderedSceneListener unregistered by {Sender.Path}");
                    _renderedSceneListeners.Remove(listener.actor);
                    listener.actor.Tell(new Received());
                    break;
                case RegisterRenderedTileListener listener:
                    Log.Info($"RegisterRenderedTileListener registered by {Sender.Path}");
                    _renderedTileListeners.Add(listener.actor);
                    listener.actor.Tell(new Received());
                    break;
                case UnregisterRenderedTileListener listener:
                    Log.Info($"UnregisterRenderedTileListener unregistered by {Sender.Path}");
                    _renderedTileListeners.Remove(listener.actor);
                    listener.actor.Tell(new Received());
                    break;
            }
        }
        
        public static Props Props() => Akka.Actor.Props.Create(() => new RaytracerActor());
    }
}
