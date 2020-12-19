using RaytrAkkar.Common;
using System;

namespace RaytrAkkar.TileRenderService
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new TileRenderService();
            service.Start();

            Console.Title = "TileRenderService";

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                service.Stop().Wait();
                eventArgs.Cancel = true;
            };

            service.UntilTerminated.Wait();
        }
    }
}
