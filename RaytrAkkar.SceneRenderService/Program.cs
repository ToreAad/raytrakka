using RaytrAkkar.Common;
using System;

namespace RaytrAkkar.SceneRenderService
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SceneRenderService();
            service.Start();

            Console.Title = "SceneRenderService";

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                service.Stop().Wait();
                eventArgs.Cancel = true;
            };

            service.UntilTerminated.Wait();

        }
    }
}
