using Akka.Actor;
using Akka.Event;
using RaytrAkkar.Raytracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RaytrAkkar.Lisp;

namespace RaytrAkkar.Common
{
    public class TileRenderActor : ReceiveActor
    {
        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public TileRenderActor()
        {
            Become(Ready);
        }

        protected override void PreStart() => Log.Info($"TileRenderActor started");
        protected override void PostStop() => Log.Info($"TileRenderActor stopped");


        private Vec3 Color(Ray r, IHitable world, int depth)
        {
            var rec = new HitRecord();
            if (world.Hit(r,0.001, double.MaxValue, ref rec))
            {
                if(depth < 50 && rec.material.Scatter(r, ref rec, out var attenuation, out var scattered))
                {
                    return attenuation * Color(scattered, world, depth +1);
                }
                else
                {
                    return new Vec3(0, 0, 0);
                }

            }
            else
            {
                var unitDirection = r.Direction.MakeUnitVector();
                var t = 0.5 * (unitDirection.Y + 1.0);
                return (1.0 - t) * (new Vec3(1.0, 1.0, 1.0)) + t * new Vec3(0.5, 0.7, 1.0);
            }
        }

        private byte[,,] DoRendering(RenderTile tile)
        {
            var simpleScene = GetScene.SimpleScene(tile.Tile.Scene.Src);
            var scene = tile.Tile.Scene;
            var x = tile.Tile.X;
            var y = tile.Tile.Y;
            var h = tile.Tile.Height;
            var w = tile.Tile.Width;
            var data = new byte[w, h, 3];

            var world = simpleScene.World;
            //var world = new HitableCollection();
            //world.List.Add(new Sphere(new Vec3(0, 0, -1), 0.5, new Lambertian(new Vec3(0.1, 0.2, 0.5))));
            //world.List.Add(new Sphere(new Vec3(0, -100.5, -1), 100, new Lambertian(new Vec3(0.8, 0.8, 0.0))));
            //world.List.Add(new Sphere(new Vec3(1, 0, -1), 0.5, new Metal(new Vec3(0.8, 0.6, 0.2), 0)));
            //world.List.Add(new Sphere(new Vec3(-1, 0, -1), 0.5, new Dielectric(1.5)));
            ////world.List.Add(new Sphere(new Vec3(-1, 0, -1), -0.45, new Dielectric(1.5)));

            //var lookFrom = new Vec3(3, 3, 2);
            //var lookAt = new Vec3(0, 0, -1);
            var lookFrom = simpleScene.From;
            var lookAt = simpleScene.To;

            var cam = new Camera(lookFrom, lookAt, new Vec3(0, 1, 0), 20, (double)(w + scene.Width) / (double)(h + scene.Height), 0.5, (lookFrom - lookAt).Length());
            var ns = 25;
            for (int dx = 0; dx < w; dx++)
            {
                for (int dy = 0; dy < h; dy++)
                {
                    var rnd = new Random();
                    var col = new Vec3(0, 0, 0);
                    for (int s = 0; s < ns; s++)
                    {
                        double u = (float)(dx + x + rnd.NextDouble()) / (float)(w + scene.Width);
                        double v = (float)(scene.Height - dy - y + rnd.NextDouble()) / (float)(h + scene.Height);
                        var r = cam.GetRay(u, v);
                        r.PointAtParameter(2.0);
                        col += Color(r, world, 0);
                    }
                    col /= ns;
                    col = col.Sqrt();

                    data[dx, dy, 0] = (byte)(col.R * 255.99f);
                    data[dx, dy, 1] = (byte)(col.G * 255.99f);
                    data[dx, dy, 2] = (byte)(col.B * 255.99f);
                }
            }
            return data;
        }

        protected void Ready()
        {
            Receive<RenderTile>(tile =>
            {
                var sender = Sender;
                var data = DoRendering(tile);
                var msg = new RenderedTile(tile.Tile, data.Flatten());
                Sender.Tell(msg);
            });
        }

        public static Props Props() => Akka.Actor.Props.Create(() => new TileRenderActor());
    }


}
