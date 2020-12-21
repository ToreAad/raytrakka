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
        private readonly Dictionary<string, IActorRef> _sceneRenderers = new Dictionary<string, IActorRef>();

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        protected override void PreStart() => Log.Info("SceneRenderSupervisor started");
        protected override void PostStop() => Log.Info("SceneRenderSupervisor stopped");

        public SceneRenderSupervisor()
        {
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    Log.Info($"Received render scene request from {Sender.Path}");
                    var sceneRenderer = Context.ActorOf(SceneRenderActor.Props(Self));
                    _sceneRenderers.Add(scene.Scene.SceneId, sceneRenderer);
                    sceneRenderer.Forward(scene);
                    break;

                case RenderedScene rendererdScene:
                    Sender.Tell(PoisonPill.Instance);
                    _sceneRenderers.Remove(rendererdScene.Scene.SceneId);
                    break;
            }
        }

        public static Props Props() => Akka.Actor.Props.Create(() => new SceneRenderSupervisor());
    }
}
