using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Common
{
    public class SceneRenderSupervisor : UntypedActor
    {
        private readonly Dictionary<int, IActorRef> _sceneRenderers = new Dictionary<int, IActorRef>();
        private readonly Dictionary<int, IActorRef> _listeners = new Dictionary<int, IActorRef>();

        private readonly IActorRef _tileRenderer;

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        protected override void PreStart() => Log.Info("SceneRenderSupervisor started");
        protected override void PostStop() => Log.Info("SceneRenderSupervisor stopped");

        public SceneRenderSupervisor()
        {
            _tileRenderer = Context.ActorOf(TileRenderActor.Props(Self).WithRouter(FromConfig.Instance), $"tile-renderer");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    var sender = Sender;
                    if (_listeners.ContainsKey(scene.Scene.SceneId))
                    {
                        break;
                    }
                    var sceneRenderer = Context.ActorOf(SceneRenderActor.Props(scene.Scene, Self), $"scene-renderer-{scene.Scene.SceneId}");
                    _sceneRenderers.Add(scene.Scene.SceneId, sceneRenderer);
                    sceneRenderer.Tell(new Run());
                    _listeners.Add(scene.Scene.SceneId, sender);
                    break;

                case RenderedScene rendererdScene:
                    _listeners[rendererdScene.Scene.SceneId].Tell(rendererdScene);
                    Sender.Tell(PoisonPill.Instance);
                    _sceneRenderers.Remove(rendererdScene.Scene.SceneId);
                    _listeners.Remove(rendererdScene.Scene.SceneId);
                    break;

                case RenderTile tile:
                    _tileRenderer.Forward(tile);
                    break;

                case RenderedTile renderedTile:
                    _listeners[renderedTile.Tile.Scene.SceneId].Tell(renderedTile);
                    break;
            }
        }

        public static Props Props() => Akka.Actor.Props.Create(() => new SceneRenderSupervisor());
    }
}
