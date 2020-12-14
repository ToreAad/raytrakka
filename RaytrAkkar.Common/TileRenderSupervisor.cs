using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytrAkkar.Common
{
    class TileRenderSupervisor : UntypedActor
    {
        public ILoggingAdapter Log { get; } = Context.GetLogger();


        protected override void PreStart() => Log.Info($"TileRenderSupervisor started");
        protected override void PostStop() => Log.Info($"TileRenderSupervisor stopped");

        public TileRenderSupervisor()
        {

        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderTile tile:
                    {
                        var tileRenderer = Context.ActorOf(TileRenderActor.Props(), $"tile-renderer-{tile.Tile.Scene.SceneId}-{tile.Tile.TileId}");
                        tileRenderer.Tell(tile);
                        break;
                    }
                case RenderedTile renderedTile:
                    {
                        Context.Parent.Tell(renderedTile);
                        Sender.Tell(PoisonPill.Instance);
                        break;
                    }

                case FailedRenderTile failedTile:
                    {
                        Sender.Tell(PoisonPill.Instance);
                        var tileRenderer = Context.ActorOf(TileRenderActor.Props(), $"tile-renderer-{failedTile.tile.Tile.Scene.SceneId}-{failedTile.tile.Tile.TileId}");
                        tileRenderer.Tell(failedTile.tile);
                        break;
                    }

            }
        }

        public static Props Props() => Akka.Actor.Props.Create(() => new TileRenderSupervisor());
    }
}
