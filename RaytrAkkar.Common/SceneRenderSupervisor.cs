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

        private readonly IActorRef _tileRenderer;
        private readonly IActorRef _supervisor;

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        protected override void PreStart() => Log.Info("SceneRenderSupervisor started");
        protected override void PostStop() => Log.Info("SceneRenderSupervisor stopped");

        public SceneRenderSupervisor(IActorRef supervisor)
        {
            _tileRenderer = Context.ActorOf(TileRenderActor.Props(Self).WithRouter(FromConfig.Instance), $"tile-renderer");
            _supervisor = supervisor;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    var sceneRenderer = Context.ActorOf(SceneRenderActor.Props(scene.Scene, Self), $"scene-renderer-{scene.Scene.SceneId}");
                    _sceneRenderers.Add(scene.Scene.SceneId, sceneRenderer);
                    sceneRenderer.Tell(new Run());
                    break;

                case RenderedScene rendererdScene:
                    _supervisor.Tell(rendererdScene);
                    Sender.Tell(PoisonPill.Instance);
                    _sceneRenderers.Remove(rendererdScene.Scene.SceneId);
                    break;

                case RenderTile tile:
                    _tileRenderer.Forward(tile);
                    break;

                case RenderedTile renderedTile:
                    _supervisor.Tell(renderedTile);
                    break;
            }
        }

        public static Props Props(IActorRef supervisor) => Akka.Actor.Props.Create(() => new SceneRenderSupervisor(supervisor));
    }
}
