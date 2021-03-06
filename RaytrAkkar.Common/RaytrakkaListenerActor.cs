﻿using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using RaytrAkkar.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Common
{
    public class RaytrakkaListenerActor : UntypedActor
    {
        private IRaytrakkaBridge _bridge;

        private readonly IActorRef _raytracer;

        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public RaytrakkaListenerActor(IRaytrakkaBridge bridge)
        {
            _bridge = bridge;
            var props = RaytracerActor.Props().WithRouter(FromConfig.Instance);
            _raytracer = Context.ActorOf(props, "raytracer");
            _raytracer.Tell(new RegisterRenderedSceneListener { actor = Self});
            _raytracer.Tell(new RegisterRenderedTileListener { actor = Self });
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderedScene scene:
                    _bridge.AddRenderedScene(scene);
                    break;
                case RenderedTile tile:
                    _bridge.AddRenderedTile(tile);
                    break;
                case RenderScene scene:
                    _raytracer.Tell(scene);
                    _bridge.AddScene(scene);
                    break;
                case RenderSceneFailed failedScene:
                    _bridge.Log(failedScene.ErrorMsg);
                    break;
                case string msg:
                    _bridge.Log(msg);
                    break;
            }
        }

        public static Props Props(IRaytrakkaBridge bridge) => Akka.Actor.Props.Create(() => new RaytrakkaListenerActor(bridge));
    }
}
