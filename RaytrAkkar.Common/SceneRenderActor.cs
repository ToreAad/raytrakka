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
        private readonly Queue<Tile> _tilesToProcess = new Queue<Tile>();
        private readonly HashSet<Tile> _unconfirmedTilesToProcess = new HashSet<Tile>();
        private readonly HashSet<Tile> _tilesBeingProcessed = new HashSet<Tile>();
        private readonly HashSet<Tile> _SetOfTilesToProcess = new HashSet<Tile>();
        private IActorRef _supervisor;
        private IActorRef _sender;
        private readonly IActorRef _tileRenderer;
        private DateTime _lastReceived;

        private readonly int _nrOfSimultaneousJobs = 8;

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public SceneRenderActor(IActorRef supervisor)
        {
            _supervisor = supervisor;
            _tileRenderer = Context.ActorOf(TileRenderActor.Props().WithRouter(FromConfig.Instance), $"tile-renderer");
            try
            {
                var strNrsOfSimultaneousJobs = Environment.GetEnvironmentVariable("nrsimultaneousjobs");
                _nrOfSimultaneousJobs = Int32.Parse(strNrsOfSimultaneousJobs);
            }
            catch
            {
                _nrOfSimultaneousJobs = 8;
            }
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderScene scene:
                    {
                        Log.Info($"Received render scene request from {Sender.Path}");
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
                                _tilesToProcess.Enqueue(tile);
                                _SetOfTilesToProcess.Add(tile);
                                tileIndex++;
                            }
                        }

                        Self.Tell(new PleaseRenderSomeTilesRequest(_nrOfSimultaneousJobs));
                        break;
                    }

                case PleaseRenderSomeTilesRequest req:
                    {
                        foreach (var _ in Enumerable.Range(0, req.Amount))
                        {
                            var tile = _tilesToProcess.Dequeue();
                            _unconfirmedTilesToProcess.Add(tile);
                            var tileRenderRequest = new RenderTile(tile);
                            _tileRenderer.Tell(tileRenderRequest);
                        }

                        _lastReceived = DateTime.Now;

                        Context.System.Scheduler.ScheduleTellRepeatedly(
                            TimeSpan.FromSeconds(0),
                            TimeSpan.FromSeconds(5),
                            Self, ConsiderResendingTiles.Instance, ActorRefs.NoSender);
                        break;
                    }

                case RenderTileRecieved tileRecieved:
                    {
                        _unconfirmedTilesToProcess.Remove(tileRecieved.Tile);
                        _tilesBeingProcessed.Add(tileRecieved.Tile);
                        break;
                    }

                case ConsiderResendingTiles _:
                    {
                        var now = DateTime.Now;
                        if (now - _lastReceived > TimeSpan.FromSeconds(10))
                        {
                            _sender.Tell($"Timeout I am resending {_unconfirmedTilesToProcess.Count} missing tiles :(");
                            Log.Info("Timeout I am resending missing tiles");
                            foreach (var tile in _unconfirmedTilesToProcess.Concat(_SetOfTilesToProcess.Except(_tilesBeingProcessed)).Take(_nrOfSimultaneousJobs))
                            {
                                _unconfirmedTilesToProcess.Add(tile);
                                var tileRenderRequest = new RenderTile(tile);
                                _tileRenderer.Tell(tileRenderRequest);
                            }
                            _lastReceived = DateTime.Now + TimeSpan.FromSeconds(5);
                        }
                        break;
                    }

                case RenderedTile tile:
                    {
                        _lastReceived = DateTime.Now;

                        if(_tilesToProcess.Count() > 0)
                        {
                            var newTile = _tilesToProcess.Dequeue();
                            _unconfirmedTilesToProcess.Add(newTile);
                            var tileRenderRequest = new RenderTile(newTile);
                            _tileRenderer.Tell(tileRenderRequest);
                        }
                        
                        Log.Info($"Recieved tile with scene-tile-id:{tile.Tile.Scene.SceneId}-{tile.Tile.TileId}");
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

                        _tilesBeingProcessed.Remove(tile.Tile);
                        _SetOfTilesToProcess.Remove(tile.Tile);
                        if (_SetOfTilesToProcess.Count == 0)
                        {
                            var msg = new RenderedScene(_scene, _values.Flatten());
                            _supervisor.Tell(msg);
                            _sender.Tell(msg);
                        }
                        break;
                    }

                case string str:
                    {
                        _sender.Tell(str);
                        break;
                    }

                case RenderTileFailed failedTile:
                    {
                        _sender.Tell(new RenderSceneFailed(_scene, failedTile.ErrorMsg));
                        Self.Tell(PoisonPill.Instance);
                        break;
                    }
            }
        }

        public static Props Props(IActorRef supervisor) => Akka.Actor.Props.Create(() => new SceneRenderActor(supervisor));
    }
}
