using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public struct HitRecord
    {
        public double t;
        public Vec3 p;
        public Vec3 normal;
        public IMaterial material;
    }

    public interface IHitable
    {
        bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec);
    }
}
