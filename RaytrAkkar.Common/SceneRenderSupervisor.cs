using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Common
{
    class SceneRenderSupervisor : UntypedActor
    {
        private readonly Dictionary<int, IActorRef> _sceneRenderers = new Dictionary<int, IActorRef>();

        private readonly IActorRef _tileRenderSupervisor;
        private readonly List<IActorRef> _renderedTileListeners = new List<IActorRef>();

        public ILoggingAdapter Log { get; } = Context.GetLogger();


        protected override void PreStart() => Log.Info("SceneRenderSupervisor started");
        protected override void PostStop() => Log.Info("SceneRenderSupervisor stopped");

        public SceneRenderSupervisor()
        {
            _tileRenderSupervisor = Context.ActorOf(TileRenderSupervisor.Props(), $"tile-render-supervisor"); ;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    var sceneRenderer = Context.ActorOf(SceneRenderActor.Props(scene.Scene), $"scene-renderer-{scene.Scene.SceneId}");
                    _sceneRenderers.Add(scene.Scene.SceneId, sceneRenderer);
                    sceneRenderer.Tell(new Run());
                    break;

                case RenderedScene rendererdScene:
                    Context.Parent.Tell(rendererdScene);
                    Sender.Tell(PoisonPill.Instance);
                    _sceneRenderers.Remove(rendererdScene.Scene.SceneId);
                    break;

                case RenderTile tile:
                    _tileRenderSupervisor.Tell(tile);
                    break;

                case RenderedTile renderedTile:
                    var destinationSceneRenderer = _sceneRenderers[renderedTile.Tile.Scene.SceneId];
                    destinationSceneRenderer.Tell(renderedTile);
                    foreach (var listener in _renderedTileListeners)
                    {
                        listener.Tell(renderedTile);
                    }
                    break;

                case RegisterRenderedTileListener listener:
                    _renderedTileListeners.Add(listener.actor);
                    break;
                case UnregisterRenderedTileListener listener:
                    _renderedTileListeners.Remove(listener.actor);
                    break;
            }
        }

        public static Props Props() => Akka.Actor.Props.Create(() => new SceneRenderSupervisor());
    }
}
