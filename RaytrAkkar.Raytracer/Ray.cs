using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class Ray
    {
        private readonly Vec3 _a;
        private readonly Vec3 _b;

        public Ray(Vec3 a, Vec3 b)
        {
            _a = a;
            _b = b;
        }

        public Vec3 A => _a;
        public Vec3 B => _b;
        public Vec3 Origin => _a;
        public Vec3 Direction => _b;

        public Vec3 PointAtParameter(double t) => _a + t * B;

    }
}
