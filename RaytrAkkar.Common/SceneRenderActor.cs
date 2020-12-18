using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaytrAkkar.Common
{
    public class SceneRenderActor : UntypedActor
    {
        private Scene _scene;
        private byte[,,] _values;
        private readonly HashSet<int> _tilesBeingProcessed = new HashSet<int>();
        private IActorRef _supervisor;
        private IActorRef _sender;
        private readonly IActorRef _tileRenderer;

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public SceneRenderActor(IActorRef supervisor)
        {
            
            _supervisor = supervisor;
            _tileRenderer = Context.ActorOf(TileRenderActor.Props(Self).WithRouter(FromConfig.Instance), $"tile-renderer");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    {
                        _scene = scene.Scene;
                        _sender = Sender;
                        _values = new byte[_scene.Width, _scene.Height, 3];
                        int w = 64;
                        int h = 64;
                        int tileIndex = 0;
                        for (int x = 0; x < _scene.Width; x += w)
                        {
                            for (int y = 0; y < _scene.Height; y += h)
                            {
                                var _h = Math.Min(h, _scene.Height - y);
                                var _w = Math.Min(w, _scene.Width - x);
                                var tile = new Tile(_scene, tileIndex, x, y, _h, _w);
                                var tileRenderRequest = new RenderTile(tile);
                                _tileRenderer.Tell(tileRenderRequest);
                                _tilesBeingProcessed.Add(tileIndex);
                                tileIndex++;
                            }
                        }
                        break;
                    }

                case RenderedTile tile:
                    {
                        _sender.Forward(tile);
                        int w = tile.Tile.Width;
                        int h = tile.Tile.Height;
                        var data = tile.Data.Unflatten(w, h, 3);
                        for (int dx = 0; dx <  w; dx++)
                        {
                            for(int dy = 0; dy < h; dy++)
                            {
                                for(int c = 0; c < 3; c++)
                                {
                                    _values[tile.Tile.X + dx, tile.Tile.Y + dy, c] = data[dx, dy, c];
                                }
                            }
                        }

                        _tilesBeingProcessed.Remove(tile.Tile.TileId);
                        if(_tilesBeingProcessed.Count == 0)
                        {
                            var msg = new RenderedScene(_scene, _values.Flatten());
                            _supervisor.Tell(msg);
                            _sender.Tell(msg);
                        }
                        break;
                    }
            }
        }

        public static Props Props(IActorRef supervisor) => Akka.Actor.Props.Create(() => new SceneRenderActor(supervisor));
    }
}
