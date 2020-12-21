using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class SimpleScene
    {
        public SimpleScene(IHitable world, Vec3 to, Vec3 from, int width = 1024, int height = 768)
        {
            World = world;
            To = to;
            From = from;
            Width = width;
            Height = height;
        }

        public IHitable World { get; set; }
        public Vec3 To { get; set; }
        public Vec3 From { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
