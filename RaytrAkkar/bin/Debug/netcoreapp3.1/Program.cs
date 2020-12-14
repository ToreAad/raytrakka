using Akka.Actor;
using Akka.Configuration;
using RaytrAkkar.Common;
using System;
using System.Linq;

namespace RaytrAkkar
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.Load();
            using (var system = ActorSystem.Create("raytracer-system", config))
            {
                var imageWriter = system.ActorOf(WriteImageToDiskActor.Props(), "image-writer");
                var raytracer = system.ActorOf(RaytracerActor.Props(), "raytracer");
                raytracer.Tell(new RegisterRenderedSceneListener { actor = imageWriter });
                //var scene = new Scene(0, 512, 256);
                //raytracer.Tell(new RenderScene(scene));
                Console.WriteLine("Press a key to quit");
                Console.ReadKey();
            }
        }
    }
}
