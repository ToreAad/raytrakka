using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaytrAkkar.Common
{
    public class EmptyService
    {
        private ActorSystem _clusterSystem;

        public Task UntilTerminated => _clusterSystem.WhenTerminated;

        public bool Start()
        {
            var config = ConfigurationFactory.Load();
            _clusterSystem = ActorSystem.Create("raytrakkar", config);
            return true;
        }

        public async Task Stop() => await CoordinatedShutdown.Get(_clusterSystem).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance);

    }
}
