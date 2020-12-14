using System;
using System.Collections.Generic;
using System.Text;

namespace RaytrAkkar.Raytracer
{
    public class Sphere : IHitable
    {
        private readonly Vec3 _center;
        private readonly double _radius;
        private readonly IMaterial _material;

        public Sphere(Vec3 center, double radius, IMaterial material)
        {
            _center = center;
            _radius = radius;
            _material = material;
        }

        public bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            var oc = r.Origin - _center;
            var a = r.Direction.Dot(r.Direction);
            var b = oc.Dot(r.Direction);
            var c = oc.Dot(oc) - _radius * _radius;

            var discriminant = b * b -  a * c;

            if (discriminant > 0)
            {
                double temp = (-b - Math.Sqrt(b * b - a * c)) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.t = temp;
                    rec.p = r.PointAtParameter(rec.t);
                    rec.normal = (rec.p - _center) / _radius;
                    rec.material = _material;
                    return true;
                }
            }
            return false;
        }
    }
}
