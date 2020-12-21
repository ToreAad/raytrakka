using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Common
{
    public class RenderedTile
    {
        public RenderedTile(Tile tile, byte[] data)
        {
            Tile = tile;
            Data = data;
        }

        public Tile Tile { get; }
        public byte[] Data { get; }
    }

    public class RegisterRenderedSceneListener
    {
        public IActorRef actor;
    }

    public class UnregisterRenderedSceneListener
    {
        public IActorRef actor;
    }

    public class RegisterRenderedTileListener
    {
        public IActorRef actor;
    }

    public class Received
    {

    }

    public class FailedRenderTile
    {
        public RenderTile tile;

        public FailedRenderTile(RenderTile tile)
        {
            this.tile = tile;
        }
    }

    public class Cancel
    {
    }

    public class UnregisterRenderedTileListener
    {
        public IActorRef actor;
    }

    public class Tile
    {
        public Tile(Scene scene, int tileId, int x, int y, int height, int width)
        {
            Scene = scene;
            X = x;
            Y = y;
            Height = height;
            Width = width;
            TileId = tileId;
        }

        public int TileId { get; }
        public Scene Scene { get; }
        public int X { get; }
        public int Y { get; }
        public int Height { get; }
        public int Width { get; }

        public override bool Equals(object obj)
        {
            return obj is Tile tile &&
                   TileId == tile.TileId &&
                   EqualityComparer<Scene>.Default.Equals(Scene, tile.Scene) &&
                   X == tile.X &&
                   Y == tile.Y &&
                   Height == tile.Height &&
                   Width == tile.Width;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TileId, Scene, X, Y, Height, Width);
        }
    }

    public class RenderTile
    {
        public RenderTile(Tile tile)
        {
            Tile = tile;
        }

        public Tile Tile { get; }
    }

    public class RenderedScene
    {
        public RenderedScene(Scene scene, byte[] data)
        {
            Scene = scene;
            Data = data;
        }

        public Scene Scene { get; }
        public byte[] Data { get; }
    }

    public class RenderScene
    {
        public RenderScene(Scene scene)
        {
            Scene = scene;
        }

        public Scene Scene { get; }
    }

    public class Scene
    {
        public Scene(string sceneId, int width, int height)
        {
            SceneId = sceneId;
            Width = width;
            Height = height;
        }

        public string SceneId { get; }
        public int Width { get; }
        public int Height { get; }
        public string Src { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Scene scene &&
                   SceneId == scene.SceneId &&
                   Width == scene.Width &&
                   Height == scene.Height &&
                   Src == scene.Src;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SceneId, Width, Height, Src);
        }
    }

    public class Run
    {

    }

    public class ConsiderResendingTiles
    {
        public static ConsiderResendingTiles Instance { get; } = new ConsiderResendingTiles();
        private ConsiderResendingTiles() { }
    }

    public class PoolPrimer
    {
        public static PoolPrimer Instance { get; } = new PoolPrimer();
        private PoolPrimer() { }
    }
}
