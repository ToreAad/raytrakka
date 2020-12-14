using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RaytrAkkar.Common
{
    public class WriteImageToDiskActor : UntypedActor
    {
        public ILoggingAdapter Log { get; } = Context.GetLogger();

        public WriteImageToDiskActor()
        {
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case RenderedScene scene:
                    var bmp = scene.ToBitmap();
                    bmp.Save($"{scene.Scene.SceneId}.png");
                    break;
            }
        }
        public static Props Props() => Akka.Actor.Props.Create(() => new WriteImageToDiskActor());
    }
}
