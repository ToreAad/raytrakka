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
            var service = new RaytracerService.RaytracerService();
            service.Start();
            Console.Title = "RaytracerService";

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                service.Stop().Wait();
                eventArgs.Cancel = true;
            };

            service.UntilTerminated.Wait();
        }
    }
}
